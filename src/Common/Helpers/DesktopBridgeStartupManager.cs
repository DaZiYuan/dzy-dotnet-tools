using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Microsoft.Win32;

namespace Common.Helpers
{
    public interface IStartupManager
    {
        Task<bool> Check();
        Task<bool> Set(bool enable);
    }

    //destopbridge用的
    public class DesktopBridgeStartupManager : IStartupManager
    {
        private readonly string _key;
        public DesktopBridgeStartupManager(string key)
        {
            _key = key;
        }

        public async Task<bool> Set(bool enabled)
        {
            try
            {
                var startupTask = await StartupTask.GetAsync(_key);
                if (!enabled && startupTask.State == StartupTaskState.Enabled)
                {
                    startupTask.Disable();
                    return true;
                }
                else if (enabled)
                {
                    var state = await startupTask.RequestEnableAsync();
                    switch (state)
                    {
                        case StartupTaskState.DisabledByUser:
                            return false;
                        case StartupTaskState.Enabled:
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> Check()
        {
            try
            {
                bool result = false;
                var startupTask = await StartupTask.GetAsync(_key);
                switch (startupTask.State)
                {
                    case StartupTaskState.Disabled:
                        result = false;
                        break;
                    case StartupTaskState.DisabledByUser:
                        result = false;
                        break;
                    case StartupTaskState.Enabled:
                        result = true;
                        break;
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
