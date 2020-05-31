namespace Synapse
{
    public abstract class Plugin
    {
        public static YamlConfig Config;
        public abstract string getName { get; }

        /// <summary>The Method ist always activated when the Server starts</summary>
        /// <remarks>You can use it to hook Events</remarks>
        public abstract void OnEnable();
    }
}
