using System;
using System.Collections.Generic;
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
                case ".7zp":
                    await SevenZip.ExtractAsync(sourceFile, distPath, token, progress);
                    break;
            }
        }
    }
}
