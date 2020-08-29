using System.Linq;
using Synapse.Events.Classes;
using Synapse.Config;
using UnityEngine;
using Synapse.Api;
using MEC;
using Grenades;
using Mirror;

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

            #if DEBUG
            Events.KeyPressEvent += OnKey;
            Events.ShootEvent += OnShoot;
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

        //Only Debug Events
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
                    dm.Scale = Vector3.one * 2;
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

        private void OnShoot(ShootEvent ev)
        {
            var cam = ev.Player.Hub.PlayerCameraReference;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit where, 40f);
            if (where.transform.TryGetComponent<Grenade>(out var grenade))
            {
                grenade.NetworkfuseTime = 0;
                grenade.ServersideExplosion();
            }

            else if (where.transform.TryGetComponent<Pickup>(out var pickup))
            {
                if (pickup.itemId == ItemType.GrenadeFrag)
                {
                    var pos = pickup.position;
                    pickup.position = Vector3.zero;
                    pickup.Delete();

                    var gm = Server.Host.GetComponent<GrenadeManager>();
                    var grenade2 = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFrag);
                    var component = Object.Instantiate(grenade2.grenadeInstance).GetComponent<Grenade>();
                    component.InitData(gm, Vector3.zero, Vector3.zero);
                    component.transform.position = pos;


                    NetworkServer.Spawn(component.gameObject);

                    component.NetworkfuseTime = 0f;
                    component.ServersideExplosion();
                }
            }
        }
    }
}