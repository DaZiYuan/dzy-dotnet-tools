using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Helpers
{
    public class IOHelper
    {
        public static void CopyFileToDir(string sourcePath, string dir, string? targetFileName = null)
        {
            if (!File.Exists(sourcePath))
                return;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            FileInfo file = new(sourcePath);
            if (string.IsNullOrEmpty(targetFileName))
                targetFileName = file.Name;

            string target = Path.Combine(dir, targetFileName);
            if (sourcePath == target)
                return;

            file.CopyTo(target, true);
        }
        public static void CopyFolder(string source, string target)
        {
            CopyFolder(new DirectoryInfo(source), new DirectoryInfo(target));
        }
        public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyFolder(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
