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
        /// <summary>Send a Information in the Console</summary>
        /// <param name="message">Information Message</param>
        public static void Info(string message)
        {
            var assembly = Assembly.GetCallingAssembly();
            ServerConsole.AddLog($"Information from: [{assembly.GetName().ToString().Split(',')[0]}] {message}");
        }

        /// <summary>Sends a Warn in the Console</summary>
        /// <param name="message">Warning Message</param>
        public static void Warn(string message)
        {
            var assembly = Assembly.GetCallingAssembly();
            ServerConsole.AddLog($"Warning from [{assembly.GetName().ToString().Split(',')[0]}] {message}",ConsoleColor.Yellow);
        }

        /// <summary>Sends a Error in the Console</summary>
        /// <param name="message">Error Message</param>
        public static void Error(string message)
        {
            var assembly = Assembly.GetCallingAssembly();
            ServerConsole.AddLog($"Error Message from [{assembly.GetName().ToString().Split(',')[0]}] {message}",ConsoleColor.Red);
        }
    }
}
