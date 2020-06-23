using System;
using System.IO;

namespace Synapse
{
    public abstract class Plugin
    {
        /// <summary>
        ///     The Main Config from the current Server which all Plugins can use
        /// </summary>
        // ReSharper disable once NotAccessedField.Global
        public static YamlConfig Config;

        public delegate void OnConfigReload();
        public event OnConfigReload ConfigReloadEvent;
        internal void InvokeConfigReloadEvent()
        {
            if (ConfigReloadEvent == null) return;

            ConfigReloadEvent.Invoke();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Translation Translation { get; internal set; }

        private string _ownPluginFolder;

        /// <summary>
        ///     A Directory especially for your Plugin which are created by Synapse for you!
        /// </summary>
        /// <remarks>The Name of the Directory is based on the GetName string from your Plugin!</remarks>
        public string OwnPluginFolder
        {
            // ReSharper disable once UnusedMember.Global
            get
            {
                if (!Directory.Exists(_ownPluginFolder))
                    Directory.CreateDirectory(_ownPluginFolder);

                return _ownPluginFolder + Path.DirectorySeparatorChar;
            }
            internal set => _ownPluginFolder = value;
        }

        public abstract string GetName { get; }

        /// <summary>The Method ist always activated when the Server starts</summary>
        /// <remarks>You can use it to hook Events</remarks>
        public abstract void OnEnable();
    }
}