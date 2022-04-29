using PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WinAPI.Helpers
{
    /// <summary>
    /// 屏幕相关操作
    /// </summary>
    public class ScreenHelper
    {
        private static readonly bool _multiMonitorSupport = (User32.GetSystemMetrics(User32.SystemMetric.SM_CMONITORS) != 0);//多屏

        // This identifier is just for us, so that we don't try to call the multimon
        // functions if we just need the primary monitor... this is safer for
        // non-multimon OSes.
        private const int PRIMARY_MONITOR = unchecked((int)0xBAADF00D);

        #region public methods
        /// <summary>
        /// 获取已经全屏运行的屏幕
        /// </summary>
        /// <param name="includeTaskBar">任务栏遮挡才算全屏</param>
        /// <returns></returns>
        public List<User32.MONITORINFO> GetMaximizedScreen(bool includeTaskBar = false)
        {
            throw new NotImplementedException();
        }

        public bool IsWindowMaximized(IntPtr hwnd, bool includeTaskBar = false)
        {
            var placement = User32.GetWindowPlacement(hwnd);
            if (placement.showCmd == User32.WindowShowStyle.SW_SHOWMAXIMIZED)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否有任意屏幕窗口被最大化
        /// </summary>
        /// <returns></returns>
        public bool IsAnyScreenMaximized(bool includeTaskBar = false, params int[] ignorePids)
        {
            bool EnumDesktopWindowsCallBack(IntPtr hwnd, IntPtr lParam)
            {
                //过滤的pid
                _ = User32.GetWindowThreadProcessId(hwnd, out int pid);
                if (ignorePids.Contains(pid))
                    return true;
                var maximized = IsWindowMaximized(hwnd, includeTaskBar);
                if (maximized)
                {
                    //没有多屏直接返回
                    if (!_multiMonitorSupport)
                        return true;
                    //var screen = Screen.FromHandle(hWnd);
                    //if (_maximizedScreens.Contains(screen))
                    //    return true;

                    //_maximizedScreens.Add(screen);
                    //if (Screen.AllScreens.Length == _maximizedScreens.Count)
                    //    //所有屏幕都已经全屏，不用继续检查
                    //    return false;
                    //return true;
                }
                return true;
            }

            //遍历桌面窗口
            _ = User32.EnumDesktopWindows(new User32.SafeDesktopHandle(), new User32.WNDENUMPROC(EnumDesktopWindowsCallBack), IntPtr.Zero);

            return false;
        }

        #endregion
    }
}
