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
        [TestMethod]
        public async Task GetMaximizedScreen()
        {
            ScreenHelper helper = new();
            var res = helper.GetMaximizedScreen();
            Assert.IsTrue(res == null);

            //open fullscren notepad
            ProcessStartInfo startInfo = new("notepad.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.UseShellExecute = true;
            var p = Process.Start(startInfo);
            res = helper.GetMaximizedScreen();
            await Task.Delay(1000);
            p.Kill();
            Assert.IsTrue(res != null && res.Count > 0);
        }

        [TestMethod]
        public void IsAnyScreenMaximized()
        {
            ScreenHelper helper = new();
            bool res = helper.IsAnyScreenMaximized();
        }
    }
}
