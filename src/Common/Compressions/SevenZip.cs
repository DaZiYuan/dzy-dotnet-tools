
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Compressions
{
    public class SevenZip
    {
        public static Task ExtractAsync(string sourceFile, string distPath, System.Threading.CancellationToken token, IProgress<(float compete, float total)> progress)
        {
            return Task.Run(() => Extract(sourceFile, distPath, token, progress));
        }

        public static void Extract(string sourceFile, string distPath, System.Threading.CancellationToken token, IProgress<(float compete, float total)> progress)
        {
            long _total;
            long _completed;

            _total = 0;
            _completed = 0;
            using var archive = SevenZipArchive.Open(sourceFile);
            _total = archive.Entries.Sum(m => m.Size);
            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
            {
                token.ThrowIfCancellationRequested();
                entry.WriteToDirectory(distPath, new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });

                _completed += entry.Size;

                progress?.Report(((float)_completed, _total));
            }
        }

        public static Task<bool> CanOpenAsync(string file)
        {
            return Task.Run(() => CanOpen(file));
        }

        public static bool CanOpen(string downloadFile)
        {
            try
            {
                using var archive = SevenZipArchive.Open(downloadFile);
                return archive.Entries.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
