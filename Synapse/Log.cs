using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Synapse
{
    public static class Log
    {
        public static void Info(string message)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            ServerConsole.AddLog($"[INFO] [{assembly.GetName()}] {message}");
        }

        public static void Warn(string message)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            ServerConsole.AddLog($"[WARN] [{assembly.GetName()}] {message}",ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            ServerConsole.AddLog($"[ERROR] [{assembly.GetName()}] {message}",ConsoleColor.Red);
        }
    }
}
