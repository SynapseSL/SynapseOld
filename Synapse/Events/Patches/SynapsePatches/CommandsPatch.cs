using CommandSystem;
using CommandSystem.Commands;
using Harmony;

namespace Synapse.Events.Patches.SynapsePatches
{
    [HarmonyPatch(typeof(GameConsoleCommandHandler), nameof(GameConsoleCommandHandler.LoadGeneratedCommands))]
    static class GameCommandsPatch
    {
        public static bool Prefix(GameConsoleCommandHandler __instance)
        {
            __instance.RegisterCommand(new CommandSystem.Commands.ArgsCommand());
            __instance.RegisterCommand(new CommandSystem.Commands.BuildInfoCommand());
            __instance.RegisterCommand(CommandSystem.Commands.ConfigCommand.Create());
            __instance.RegisterCommand(new CommandSystem.Commands.HelpCommand(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(ClientCommandHandler), nameof(ClientCommandHandler.LoadGeneratedCommands))]
    static class ClientCommandPatch
    {
        public static bool Prefix(GameConsoleCommandHandler __instance)
        {
            __instance.RegisterCommand(new HelpCommand(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(RemoteAdminCommandHandler), nameof(RemoteAdminCommandHandler.LoadGeneratedCommands))]
    static class RemoteCommandsPatch
    {
        public static bool Prefix(GameConsoleCommandHandler __instance)
        {
            __instance.RegisterCommand(new CommandSystem.Commands.BuildInfoCommand());
            __instance.RegisterCommand(new CommandSystem.Commands.ChangeNameCommand());
            __instance.RegisterCommand(CommandSystem.Commands.ConfigCommand.Create());
            __instance.RegisterCommand(new CommandSystem.Commands.HelpCommand(__instance));
            __instance.RegisterCommand(new CommandSystem.Commands.IntercomTextCommand());
            return false;
        }
    }
}
