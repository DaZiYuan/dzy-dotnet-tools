using PInvoke;
using System.Runtime.InteropServices;

namespace Common.WinAPI.Native
{
    internal class NativeUser32Ex
    {
        #region native 替代 pinvoke支持还不够好的方法
        internal delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, int dwData);
        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, int dwData);
        [DllImport("user32.dll")]
        internal static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] User32.MONITORINFO monitorInfo);
        #endregion
    }
}
