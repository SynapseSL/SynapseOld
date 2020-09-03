using CommandSystem;
using Synapse.Api;
using Synapse.Config;
using System;

namespace Synapse.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ReloadPermissionsCommand : ICommand
    {
        public string Command { get; } = "reloadpermissions";

        public string[] Aliases { get; } = new string[]
        {
            "rp",
            "reloadp"
        };

        public string Description { get; } = "A Command to Relaod the Permissions of Synapse";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string respone)
        {
            if (!sender.GetPlayer().CheckPermission("sy.reload.permission"))
            {
                respone = "You have no Permission for Reload Permissions";
                return false;
            }

            PermissionReader.ReloadPermission();
            respone = "Permissions Reloaded!";
            return true;
        }
    }
}
