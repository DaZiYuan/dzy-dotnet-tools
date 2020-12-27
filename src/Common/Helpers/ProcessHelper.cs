using System;
using System.IO;

namespace Common.Helpers
{
    public class ProcessHelper
    {
        public static void AddPathToEnvoirment(string dir)
        {
            var name = "PATH";
            var scope = EnvironmentVariableTarget.Process;
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            if (oldValue != null && dir != null && oldValue.Contains(dir))
                //已经有了
                return;
            var newValue = oldValue + $";{dir}";
            Environment.SetEnvironmentVariable(name, newValue, scope);
        }

        public static void RemovePathFromEnviorment(string dir)
        {
            var name = "PATH";
            var scope = EnvironmentVariableTarget.Process;
            var oldValue = Environment.GetEnvironmentVariable(name, scope);
            var newValue = oldValue.Replace(dir, "");
            Environment.SetEnvironmentVariable(name, newValue, scope);
        }
    }
}
