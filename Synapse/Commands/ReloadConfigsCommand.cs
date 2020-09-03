using CommandSystem;
using Synapse.Api;
using Synapse.Config;
using System;

namespace Synapse.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ReloadConfigsCommand : ICommand
    {
        public string Command { get; } = "reloadconfigs";

        public string[] Aliases { get; } = new string[]
        {
            "rc",
            "reloadc"
        };

        public string Description { get; } = "A Command to Relaod the Configs of Synapse";

        public bool Execute(ArraySegment<string> arguments,ICommandSender sender,out string respone)
        {
            if (!sender.GetPlayer().CheckPermission("sy.reload.configs"))
            {
                respone = "You have no Permission for Reload Configs";
                return false;
            }

            ConfigManager.ReloadAllConfigs();
            respone = "Configs Reloaded!";
            return true;
        }
    }
}
