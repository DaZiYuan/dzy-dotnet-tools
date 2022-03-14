using Common.Compressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UnitTestProject
{
    [TestClass]
    public class CompressionHelperTest
    {
        [TestMethod]
        public async Task Test()
        {
            try
            {
                IProgress<(float compete, float total)> Callback = new Progress<(float competed, float total)>((e) =>
                {
                    System.Diagnostics.Debug.WriteLine(e);
                });
                await CompressionHelper.Decompress(@"D:\LiveWallpaperEngineRender.7z", @"D:\LiveWallpaperEngineRender1", new System.Threading.CancellationToken(), Callback);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
