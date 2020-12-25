using Common.Compressions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace Common.Helpers
{
    public class NetworkHelper
    {
        public static NetworkHelper SharedInstance { get; private set; }

        public static int GetAvailablePort(int startingPort)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            var tcpConnectionPorts = properties.GetActiveTcpConnections()
                                .Where(n => n.LocalEndPoint.Port >= startingPort)
                                .Select(n => n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            var tcpListenerPorts = properties.GetActiveTcpListeners()
                                .Where(n => n.Port >= startingPort)
                                .Select(n => n.Port);

            //getting active udp listeners
            var udpListenerPorts = properties.GetActiveUdpListeners()
                                .Where(n => n.Port >= startingPort)
                                .Select(n => n.Port);

            var port = Enumerable.Range(startingPort, ushort.MaxValue)
                .Where(i => !tcpConnectionPorts.Contains(i))
                .Where(i => !tcpListenerPorts.Contains(i))
                .Where(i => !udpListenerPorts.Contains(i))
                .FirstOrDefault();

            return port;
        }

        public static async Task DownloadFileAsync(string uri, string distFile, CancellationToken cancellationToken, IProgress<(float compete, float total)> progress = null)
        {
            using HttpClient client = new HttpClient();
            Debug.WriteLine($"download {uri}");
            using HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            await Task.Run(() =>
            {
                var dir = Path.GetDirectoryName(distFile);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            });

            using FileStream distFileStream = new FileStream(distFile, FileMode.OpenOrCreate, FileAccess.Write);
            if (progress != null)
            {
                long length = response.Content.Headers.ContentLength ?? -1;
                await using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                byte[] buffer = new byte[4096];
                int read;
                int totalRead = 0;
                while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken).ConfigureAwait(false)) > 0)
                {
                    await distFileStream.WriteAsync(buffer.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
                    totalRead += read;
                    progress.Report((totalRead, length));
                }
                Debug.Assert(totalRead == length || length == -1);
            }
            else
            {
                await response.Content.CopyToAsync(distFileStream).ConfigureAwait(false);
            }
        }

        public static async Task DownloadAndDecompression(
            string url,
            string downloadDist,
            string decompressDist,
            bool deleteFile,
            Progress<(float competed, float total)> downloadProgress,
            Progress<(float competed, float total)> decompressProgress,
            CancellationToken cancellationToken
            )
        {
            if (!File.Exists(downloadDist) || !await CompressionHelper.CanOpen(downloadDist))
            {
                await DownloadFileAsync(url, downloadDist, cancellationToken, downloadProgress);
            }
            await CompressionHelper.Decompress(downloadDist, decompressDist, cancellationToken, decompressProgress);

            if (deleteFile)
            {
                await Task.Run(() => File.Delete(downloadDist), cancellationToken);
            }
        }
    }
}
