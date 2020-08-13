using CommandSystem;
using System;
using System.Linq;

namespace Synapse.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PluginInfoCommand : ICommand
    {
        public string Command { get; } = "plugin";

        public string[] Aliases { get; } = new string[]
        {
            "plugininfo",
            "pi"
        };

        public string Description { get; } = "Gives you Informations about a Plugin";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string respone)
        {
            if (arguments.Count < 1)
            {
                respone = "You also have to enter a plugin name: plugin ExamplePlugin";
                return false;
            }

            foreach(var plugin in Synapse.Plugins)
                if (plugin.Name.ToLower().Contains(arguments.FirstOrDefault().ToLower()))
                {
                    respone = $"The Plugin {plugin.Name} Version {plugin.Version} was created by {plugin.Author} and was made for Synapse v.{plugin.SynapseMajor}.{plugin.SynapseMinor}.{plugin.SynapsePatch}" +
                        $"\nPlugin Description: {plugin.Description}";
                    return true;
                }

            respone = "No Plugin with such a name was found!";
            return false;
        }
    }
}
