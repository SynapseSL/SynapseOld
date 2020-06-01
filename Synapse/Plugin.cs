using System.IO;

namespace Synapse
{
    public abstract class Plugin
    {
        private string _ownPluginFolder; 

        /// <summary>
        /// The Main Config from the current Server which all Plugins can use
        /// </summary>
        public static YamlConfig Config;

        /// <summary>
        /// A Directory especially for your Plugin which are created by Synapse for you!
        /// </summary>
        ///<remarks>The Name of the Directory is based on the GetName string from your Plugin!</remarks>
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
