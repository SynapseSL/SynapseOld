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

        private string _ownPluginFolder;

        private string _owntransTranslationFile;

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

        public string OwnTranslationFile
        {
            // ReSharper disable once UnusedMember.Global
            get
            {
                if (!File.Exists(_owntransTranslationFile))
                    File.Create(_owntransTranslationFile);

                return OwnTranslationFile;
            }
            internal set => _owntransTranslationFile = value;
        }

        public abstract string GetName { get; }

        /// <summary>The Method ist always activated when the Server starts</summary>
        /// <remarks>You can use it to hook Events</remarks>
        public abstract void OnEnable();
    }
}