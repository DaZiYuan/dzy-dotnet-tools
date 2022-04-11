using Common.Compressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UnitTestProject
{
    [TestClass]
    public class CompressionHelperTest
    {
        [TestMethod]
        public async Task Decompress7z()
        {
            try
            {
                float percent = 0;
                IProgress<(float compete, float total)> Callback = new Progress<(float competed, float total)>((e) =>
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    percent = e.competed / e.total;
                });
                string distDir = Path.Combine(Path.GetTempPath(), "1");
                if (Directory.Exists(distDir))
                    Directory.Delete(distDir, true);
                await CompressionHelper.Decompress(@"Assets/1.7z", distDir, new System.Threading.CancellationToken(), Callback);
                Assert.IsTrue(Directory.Exists(distDir));
                Assert.IsTrue(percent == 1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
