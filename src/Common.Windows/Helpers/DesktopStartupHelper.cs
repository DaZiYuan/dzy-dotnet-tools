using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Common.Windows.Helpers
{
    public class DesktopStartupHelper : IStartupManager
    {
        // see https://stackoverflow.com/questions/12945805/odd-c-sharp-path-issue
        // net core 获取出来是dll，而framework是exe。所以暴露出来让外面传
        //private readonly string ExecutablePath = System.Reflection.Assembly.GetEntryAssembly().Location;

        private readonly string _executablePath;
        private readonly string _Key;

        public DesktopStartupHelper(string key, string executablePath)
        {
            _executablePath = executablePath;
            _Key = key;
        }

        private string GetMd5(string filename)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private RegistryKey OpenRegKey(string name, bool writable, RegistryHive hive = RegistryHive.CurrentUser)
        {
            // we are building x86 binary for both x86 and x64, which will
            // cause problem when opening registry key
            // detect operating system instead of CPU
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            try
            {
                RegistryKey userKey = RegistryKey.OpenBaseKey(hive,
                        Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32)
                    .OpenSubKey(name, writable);
                return userKey;
            }
            catch (ArgumentException ae)
            {
                System.Diagnostics.Debug.WriteLine(ae);
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        public Task<bool> Set(bool enabled)
        {
            RegistryKey runKey = null;
            try
            {
                runKey = OpenRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (runKey == null)
                {
                    return Task.FromResult(false);
                }
                if (enabled)
                {
                    runKey.SetValue(_Key, _executablePath);
                }
                else
                {
                    runKey.DeleteValue(_Key);
                }
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Task.FromResult(false);
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                        runKey.Dispose();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
            }
        }

        public Task<bool> Check()
        {
            RegistryKey runKey = null;
            try
            {
                runKey = OpenRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (runKey == null)
                {
                    return Task.FromResult(false);
                }
                string[] runList = runKey.GetValueNames();
                foreach (string item in runList)
                {
                    if (item.Equals(_Key, StringComparison.OrdinalIgnoreCase))
                        return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Task.FromResult(false);
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                        runKey.Dispose();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
            }
        }
    }
}
