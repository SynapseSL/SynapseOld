using CommandSystem;
using CommandSystem.Commands;
using Harmony;

namespace Synapse.Events.Patches.SynapsePatches
{
    [HarmonyPatch(typeof(GameConsoleCommandHandler), nameof(GameConsoleCommandHandler.LoadGeneratedCommands))]
    internal static class GameCommandsPatch
    {
        public static bool Prefix(GameConsoleCommandHandler __instance)
        {
            __instance.RegisterCommand(new ArgsCommand());
            __instance.RegisterCommand(new BuildInfoCommand());
            __instance.RegisterCommand(ConfigCommand.Create());
            __instance.RegisterCommand(new HelpCommand(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(ClientCommandHandler), nameof(ClientCommandHandler.LoadGeneratedCommands))]
    internal static class ClientCommandPatch
    {
        public static bool Prefix(GameConsoleCommandHandler __instance)
        {
            __instance.RegisterCommand(new HelpCommand(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(RemoteAdminCommandHandler), nameof(RemoteAdminCommandHandler.LoadGeneratedCommands))]
    internal static class RemoteCommandsPatch
    {
        public static bool Prefix(GameConsoleCommandHandler __instance)
        {
            __instance.RegisterCommand(new BuildInfoCommand());
            __instance.RegisterCommand(new ChangeNameCommand());
            __instance.RegisterCommand(ConfigCommand.Create());
            __instance.RegisterCommand(new HelpCommand(__instance));
            __instance.RegisterCommand(new IntercomTextCommand());
            return false;
        }
    }
}
