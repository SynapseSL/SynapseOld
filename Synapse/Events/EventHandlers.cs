using System.Linq;
using Synapse.Events.Classes;
using Synapse.Config;
using UnityEngine;
using Synapse.Api;

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
    }
}