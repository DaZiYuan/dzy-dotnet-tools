using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Compressions
{
    public class CompressionHelper
    {
        public static async Task Decompress(string sourceFile, string distPath, System.Threading.CancellationToken token, IProgress<(float compete, float total)> progress)
        {
            string extension = Path.GetExtension(sourceFile);
            switch (extension)
            {
                case ".7z":
                    await SevenZip.ExtractAsync(sourceFile, distPath, token, progress);
                    break;
                case ".zip":
                    await Zip.ExtractAsync(sourceFile, distPath, token, progress);
                    break;
            }
        }

        internal static Task<bool> CanOpen(string file)
        {
            string extension = Path.GetExtension(file);
            switch (extension)
            {
                case ".7z":
                    return SevenZip.CanOpenAsync(file);
                case ".zip":
                    return Zip.CanOpenAsync(file);
            }
            return Task.FromResult(false);
        }
    }
}
