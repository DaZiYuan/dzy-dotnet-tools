using Common.WinAPI.Native;
using PInvoke;

namespace Common.WinAPI
{
    public class User32Ex
    {

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
            if (NativeUser32Ex.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, 0))
            {
                return tmp;
            }

            return tmp;
        }
    }
}
