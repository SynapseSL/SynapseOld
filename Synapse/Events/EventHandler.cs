using System.Collections.Generic;
using MEC;
using Synapse.Api;
using Synapse.Api.Enums;
using Synapse.Events.Classes;
using Synapse.Permissions;
using UnityEngine;

namespace Synapse.Events
{
    // ReSharper disable once UnusedType.Global
    internal class EventHandler
    {
        // Variables
        private bool _roundInProgress;

        // ReSharper disable once NotAccessedField.Local
        private int _roundTime;

        // Constructor
        public EventHandler()
        {
            Events.SyncDataEvent += OnSyncData;
            Events.RemoteCommandEvent += OnRemoteCommand;
            Events.RoundStartEvent += OnRoundStart;
            Events.RoundEndEvent += OnRoundEnd;
            Events.RoundRestartEvent += OnRoundRestart;
            Events.DoorInteractEvent += OnDoorInteract;
        }

        // Methods
        private static void OnDoorInteract(ref DoorInteractEvent ev)
        {
            if (!Configs.RemoteKeyCard) return;
            if (ev.Allow) return;

            foreach (var item in ev.Player.Items)
            {
                if (!ev.Player.Hub.inventory.GetItemByID(item.id).permissions
                    .Contains(ev.Door.permissionLevel)) continue;  //update to backwardsCompatPermissions when necessary
                ev.Allow = true;
                return;
            }
        }

        private static void OnSyncData(ref SyncDataEvent ev)
        {
            if (ev.Player.Role != RoleType.ClassD &&
                ev.Player.Role != RoleType.Scientist &&
                !(Vector3.Distance(ev.Player.Position, ev.Player.GetComponent<Escape>().worldPosition) >= Escape.radius))
                ev.Player.Hub.characterClassManager.CmdRegisterEscape();
        }

        private static void OnRemoteCommand(ref RemoteCommandEvent ev)
        {
            var args = ev.Command.Split(' ');
            switch (args[0].ToUpper())
            {
                case "RELOADPERMISSION":
                {
                    if (!ev.Player.CheckPermission("sy.reload.permission"))
                    {
                        ev.Sender.RaMessage("You have no Permission for Reload Permissions", false,
                            RaCategory.AdminTools);
                        return;
                    }

                    PermissionReader.ReloadPermission();
                    ev.Sender.RaMessage("Permissions Reloaded!", true, RaCategory.ServerConfigs);
                    return;
                }

                case "RELOADCONFIGS":
                    if (!ev.Player.CheckPermission("sy.reload.configs"))
                    {
                        ev.Sender.RaMessage("You have no Permission for Reload Configs", false,
                            RaCategory.AdminTools);
                        return;
                    }

                    PluginManager.OnConfigReload();
                    ev.Sender.RaMessage("Configs Reloaded!", true, RaCategory.ServerConfigs);
                    return;
            }
        }

        private void OnRoundStart()
        {
            Timing.RunCoroutine(RoundTime());
            _roundInProgress = true;
        }

        private void OnRoundEnd()
        {
            _roundInProgress = false;
        }

        private void OnRoundRestart()
        {
            _roundInProgress = false;
            Map.Rooms.Clear();
        }

        private IEnumerator<float> RoundTime()
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(1f);
                _roundTime++;
                if (!_roundInProgress) break;
            }
        }
    }
}