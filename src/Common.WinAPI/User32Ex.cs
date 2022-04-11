using PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.WinAPI
{
    public class User32Ex
    {
        #region native 替代 pinvoke支持还不够好的方法
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, int dwData);
        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, int dwData);
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] User32.MONITORINFO monitorInfo);
        #endregion
        public static List<User32.MONITORINFO> GetMonitorInfos()
        {
            List<User32.MONITORINFO> tmp = new();
            bool callback(IntPtr hDesktop, IntPtr hdc, ref RECT rect, int d)
            {
                var info = User32.MONITORINFO.Create();
                bool isok = User32.GetMonitorInfo(hDesktop, ref info);
                if (isok)
                    tmp.Add(info);
                return true;
            }

            if (EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, 0))
            {
                return tmp;
            }

            return tmp;
        }
    }
}
