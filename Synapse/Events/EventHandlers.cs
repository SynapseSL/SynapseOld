using System.Linq;
using Synapse.Api;
using Synapse.Api.Enums;
using Synapse.Events.Classes;
using Synapse.Config;
using UnityEngine;
using System;

namespace Synapse.Events
{
    // ReSharper disable once UnusedType.Global
    internal class EventHandlers
    {
        // Constructor
        public EventHandlers()
        {
            Events.SyncDataEvent += OnSyncData;
            Events.RemoteCommandEvent += OnRemoteCommand;
            Events.DoorInteractEvent += OnDoorInteract;
            Events.PlayerJoinEvent += OnPlayerJoin;
        }

        // Methods
        private void OnPlayerJoin(PlayerJoinEvent ev)
        {
            ev.Player.Broadcast(SynapseConfigs.JoinMessageDuration, SynapseConfigs.JoinBroadcast);
            ev.Player.GiveTextHint(SynapseConfigs.JoinTextHint, SynapseConfigs.JoinMessageDuration);
        }

        private static void OnDoorInteract(DoorInteractEvent ev)
        {
            if (!SynapseConfigs.RemoteKeyCard) return;
            if (ev.Allow) return;

            if (!ev.Player.Items.Any()) return;
            foreach (var item in ev.Player.Items)
            {
                var gameitem = ev.Player.Inventory.GetItemByID(item.id);

                if (gameitem.permissions == null || gameitem.permissions.Length == 0) continue;

                ev.Allow = gameitem.permissions.Any(p =>
                    ev.Door.backwardsCompatPermissions.TryGetValue(p, out var flag) &&
                    ev.Door.PermissionLevels.HasPermission(flag));
            }
        }

        private static void OnSyncData(SyncDataEvent ev)
        {
            if (ev.Player.Role != RoleType.ClassD &&
                ev.Player.Role != RoleType.Scientist &&
                !(Vector3.Distance(ev.Player.Position, ev.Player.GetComponent<Escape>().worldPosition) >= Escape.radius))
                ev.Player.Hub.characterClassManager.CmdRegisterEscape();
        }

        private static void OnRemoteCommand(RemoteCommandEvent ev)
        {
            var args = ev.Command.Split(' ');
            switch (args[0].ToUpper())
            {
                case "RELOADPERMISSION":
                    ev.Allow = false;
                    if (!ev.Player.CheckPermission("sy.reload.permission"))
                    {
                        ev.Sender.RaMessage("You have no Permission for Reload Permissions", false,
                            RaCategory.AdminTools);
                        return;
                    }

                    PermissionReader.ReloadPermission();
                    ev.Sender.RaMessage("Permissions Reloaded!", true, RaCategory.ServerConfigs);
                    return;

                case "RELOADCONFIGS":
                    ev.Allow = false;
                    if (!ev.Player.CheckPermission("sy.reload.configs"))
                    {
                        ev.Sender.RaMessage("You have no Permission for Reload Configs", false,
                            RaCategory.AdminTools);
                        return;
                    }

                    ConfigManager.ReloadAllConfigs();
                    ev.Sender.RaMessage("Configs Reloaded!", true, RaCategory.ServerConfigs);
                    return;
            }
        }
    }
}