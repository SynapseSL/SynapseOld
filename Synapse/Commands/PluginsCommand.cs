using CommandSystem;
using System;

namespace Synapse.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PluginsCommand : ICommand
    {
        public string Command { get; } = "plugins";

        public string[] Aliases { get; } = new string[]
        {
            "pl",
        };

        public string Description { get; } = "Gives you all Plugins installed on the Server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string respone)
        {
            var msg = "\nAll Plugins:";
            foreach(var plugin in Synapse.Plugins)
            {
                msg += $"\n{plugin.Name} Version: {plugin.Version} by {plugin.Author}";
            }

            respone = msg;
            return true;
        }
    }
}
