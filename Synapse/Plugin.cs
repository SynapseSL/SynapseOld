using System.IO;

namespace Synapse
{
    public abstract class Plugin
    {
        private string ownPluginFolder; 

        public static YamlConfig Config;

        public string OwnPluginFolder 
        { 
            get
            {
                if (!Directory.Exists(ownPluginFolder))
                    Directory.CreateDirectory(ownPluginFolder);

                return ownPluginFolder + Path.DirectorySeparatorChar.ToString();
            } 
            internal set => ownPluginFolder = value;
        }

        public abstract string GetName { get; }

        /// <summary>The Method ist always activated when the Server starts</summary>
        /// <remarks>You can use it to hook Events</remarks>
        public abstract void OnEnable();
    }
}
