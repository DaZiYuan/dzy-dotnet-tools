using Common.WinAPI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UnitTestProject
{
    [TestClass]
    public class ScreenTest
    {
        //没有遮挡的情况下测试
        [TestMethod]
        public async Task GetMaximizedScreen()
        {
            ScreenHelper helper = new();
            var res = helper.GetMaximizedScreen(false, true);
            Assert.IsTrue(res.Count == 0);

            //过滤id检查
            ProcessStartInfo startInfo = new("notepad.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.UseShellExecute = true;
            var p = Process.Start(startInfo);
            res = helper.GetMaximizedScreen(false, false, p.Id);
            await Task.Delay(1000);
            p.Kill();
            Assert.IsTrue(res.Count == 0);

            //全屏检查
            startInfo = new("notepad.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.UseShellExecute = true;
            p = Process.Start(startInfo);
            res = helper.GetMaximizedScreen();
            await Task.Delay(1000);
            p.Kill();
            Assert.IsTrue(res.Count == 0);
        }
    }
}
