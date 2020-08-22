using System.Linq;
using Synapse.Events.Classes;
using Synapse.Config;
using UnityEngine;
using Synapse.Api;
using MEC;

namespace Synapse.Events
{
    // ReSharper disable once UnusedType.Global
    internal class EventHandlers
    {
        // Constructor
        public EventHandlers()
        {
            Events.SyncDataEvent += OnSyncData;
            Events.DoorInteractEvent += OnDoorInteract;
            Events.PlayerJoinEvent += OnPlayerJoin;

            //KeyPressEvent for Testing many different things easy with a singel Key Press!
            #if DEBUG
            Events.KeyPressEvent += OnKey;
            #endif
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
            foreach (var gameItem in ev.Player.Items.Select(item => ev.Player.Inventory.GetItemByID(item.id)).Where(gameitem => gameitem.permissions != null && gameitem.permissions.Length != 0))
            {
                ev.Allow = gameItem.permissions.Any(p =>
                    Door.backwardsCompatPermissions.TryGetValue(p, out var flag) &&
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

        private void OnKey(KeyPressEvent ev)
        {
            if (ev.Key == KeyCode.Alpha1)
            {
                var dm = new Dummy(ev.Player.Position, Quaternion.identity, ev.Player.Role, "first", "First", "yellow");
                dm.Name = "second";
                dm.HeldItem = ItemType.GunLogicer;
                var pos = ev.Player.Position;
                pos.y += 2;
                dm.Position = pos;
                dm.Role = RoleType.Scientist;

                Timing.CallDelayed(2f, () =>
                {
                    dm.BadgeName = "TestBadge";
                });

                Timing.CallDelayed(5f, () =>
                {
                    dm.BadgeColor = "red";
                });

                Timing.CallDelayed(10f, () => dm.Destroy());
            }
            if (ev.Key == KeyCode.Alpha2)
            {
                var msg = "";
                foreach (var player in Player.GetAllPlayers())
                    msg += $"\n{player}";

                ev.Player.SendConsoleMessage(msg);
            }
            if (ev.Key == KeyCode.Alpha3)
            {
                var msg = "";
                foreach (var player in ReferenceHub.GetAllHubs())
                    msg += $"\n{player}";

                ev.Player.SendConsoleMessage(msg);
            }
        }
    }
}