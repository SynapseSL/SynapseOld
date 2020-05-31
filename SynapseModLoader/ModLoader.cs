using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SynapseModLoader
{
    public class ModLoader
    {
        // Token: 0x060027B3 RID: 10163 RVA: 0x000C17DC File Offset: 0x000BF9DC
        public static byte[] ReadFile(string path)
        {
            FileStream fileStream = File.Open(path, FileMode.Open);
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }
            fileStream.Close();
            return result;
        }

        // Token: 0x060027B4 RID: 10164 RVA: 0x000C1828 File Offset: 0x000BFA28
        public static void LoadModSystem()
        {
            if (loaded)
            {
                return;
            }
            ServerConsole.AddLog("Synpase Mod-Loader is now intialising.. :)", ConsoleColor.Blue);
            string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synapse");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            if (!File.Exists(Path.Combine(text, "Synapse.dll")))
            {
                ServerConsole.AddLog("Error while loading Synapse! The Synapse.dll is missing!", ConsoleColor.Red);
                return;
            }
            MethodInfo methodInfo = Assembly.Load(ReadFile(Path.Combine(text, "Synapse.dll"))).GetTypes().SelectMany((Type p) => p.GetMethods()).FirstOrDefault((MethodInfo f) => f.Name == "LoaderExecutionCode");
            if (!(methodInfo != null))
            {
                return;
            }
            methodInfo.Invoke(null, null);
            loaded = true;
        }

        // Token: 0x040022BC RID: 8892
        private static bool loaded;
    }
}