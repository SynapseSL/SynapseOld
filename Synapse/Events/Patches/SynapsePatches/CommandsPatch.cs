using CommandSystem;
using CommandSystem.Commands;
using Harmony;
using Synapse.Commands;
using System.Linq;

namespace Synapse.Events.Patches.SynapsePatches
{
    [HarmonyPatch(typeof(GameConsoleCommandHandler), nameof(GameConsoleCommandHandler.LoadGeneratedCommands))]
    internal static class GameCommandsPatch
    {
        public static void Postfix(GameConsoleCommandHandler __instance)
        {
            //Synapse Commands
            __instance.RegisterCommand(new PluginsCommand());
            __instance.RegisterCommand(new PluginInfoCommand());
            __instance.RegisterCommand(new ReloadPermissionsCommand());
            __instance.RegisterCommand(new ReloadConfigsCommand());
        }
    }

    [HarmonyPatch(typeof(ClientCommandHandler), nameof(ClientCommandHandler.LoadGeneratedCommands))]
    internal static class ClientCommandPatch
    {
        public static void Postfix(GameConsoleCommandHandler __instance)
        {
            //Synapse Commands
            __instance.RegisterCommand(new PluginsCommand());
            __instance.RegisterCommand(new PluginInfoCommand());
            __instance.RegisterCommand(new KeyPressCommand());
        }
    }

    [HarmonyPatch(typeof(RemoteAdminCommandHandler), nameof(RemoteAdminCommandHandler.LoadGeneratedCommands))]
    internal static class RemoteCommandsPatch
    {
        public static void Postfix(GameConsoleCommandHandler __instance)
        {
            //Synapse Commands
            __instance.RegisterCommand(new ReloadConfigsCommand());
            __instance.RegisterCommand(new ReloadPermissionsCommand());
            __instance.RegisterCommand(new PluginsCommand());
            __instance.RegisterCommand(new PluginInfoCommand());
        }
    }

    [HarmonyPatch(typeof(RefreshCommandsCommand), nameof(RefreshCommandsCommand.Execute))]
    internal static class RefreshCommandsPatch
    {
        public static void Postfix()
        {
            Synapse.OnReloadCommands();
        }
    }
}
