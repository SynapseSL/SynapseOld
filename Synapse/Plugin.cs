using System.IO;

namespace Synapse
{
    public abstract class Plugin
    {
        private string _ownPluginFolder; 

        public static YamlConfig Config;

        public string OwnPluginFolder 
        { 
            get
            {
                if (!Directory.Exists(_ownPluginFolder))
                    Directory.CreateDirectory(_ownPluginFolder);

                return _ownPluginFolder + Path.DirectorySeparatorChar.ToString();
            } 
            internal set => _ownPluginFolder = value;
        }

        public abstract string GetName { get; }

        /// <summary>The Method ist always activated when the Server starts</summary>
        /// <remarks>You can use it to hook Events</remarks>
        public abstract void OnEnable();
    }
}
