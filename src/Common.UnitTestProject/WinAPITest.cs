using Common.WinAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace Common.UnitTestProject
{
    [TestClass]
    public class WinAPITest
    {
        [TestMethod]
        public void GetMonitorInfos()
        {
            var infos = User32Ex.GetMonitorInfos();
            Assert.IsTrue(infos.Count > 0);
        }
    }
}
