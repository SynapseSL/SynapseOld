using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Assets._Scripts.Dissonance;
using Synapse.Api;
using Synapse.Events.Classes;
using UnityEngine;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        /// <summary>A Event which is activated when a User Joins the Server</summary>
        /// <remarks>It need to hook ref PlayerJoinEvent ev</remarks>
        public delegate void OnPlayerJoin(ref PlayerJoinEvent ev);
        public static event OnPlayerJoin PlayerJoinEvent;

        internal static void InvokePlayerJoinEvent(Player player, ref string nick)
        {
            if (PlayerJoinEvent == null) return;
            var ev = new PlayerJoinEvent
            {
                Player = player,
                Nick = nick
            };

            PlayerJoinEvent.Invoke(ref ev);

            nick = ev.Nick;
        }

        /// <summary>A Event which is activated when a user press any voice HotKey</summary>
        public delegate void OnSpeak(ref PlayerSpeakEvent ev);
        public static event OnSpeak SpeakEvent;

        internal static void InvokeSpeakEvent(DissonanceUserSetup dissonance, ref bool intercom, ref bool radio,
            ref bool scp939, ref bool scpChat, ref bool spectator)
        {
            if (SpeakEvent == null) return;

            var ev = new PlayerSpeakEvent
            {
                IntercomTalk = intercom,
                RadioTalk = radio,
                Scp939Talk = scp939,
                ScpChat = scpChat,
                SpectatorChat = spectator,
                DissonanceUserSetup = dissonance,
                Player = dissonance.gameObject.GetPlayer()
            };

            SpeakEvent.Invoke(ref ev);

            intercom = ev.IntercomTalk;
            radio = ev.RadioTalk;
            scp939 = ev.Scp939Talk;
            scpChat = ev.ScpChat;
            spectator = ev.SpectatorChat;
        }

        /// <summary>
        ///     A Event which is activated when a User leave the server
        /// </summary>
        public delegate void OnPlayerLeave(PlayerLeaveEvent ev);
        public static event OnPlayerLeave PlayerLeaveEvent;

        internal static void InvokePlayerLeaveEvent(Player player)
        {
            if (PlayerLeaveEvent == null) return;

            var ev = new PlayerLeaveEvent
            {
                Player = player
            };
            PlayerLeaveEvent.Invoke(ev);
        }
        
        // PlayerBanEvent
        public delegate void OnPlayerBanEvent(ref PlayerBanEvent ev);
        public static event OnPlayerBanEvent PlayerBanEvent;

        internal static void InvokePlayerBanEvent(Player player, int duration, ref bool allow,
            string reason, Player issuer)
        {
            if (PlayerBanEvent == null) return;

            var ev = new PlayerBanEvent
            {
                Issuer = issuer,
                Duration = duration,
                Reason = reason,
                BannedPlayer = player
            };

            PlayerBanEvent.Invoke(ref ev);

            allow = ev.Allowed;
        }
        
        //PlayerDieEvent
        public delegate void OnPlayerDeath(PlayerDeathEvent ev);
        public static event OnPlayerDeath PlayerDeathEvent;

        internal static void InvokePlayerDieEvent(Player player, Player killer, PlayerStats.HitInfo infos)
        {
            if (PlayerDeathEvent == null) return;

            var ev = new PlayerDeathEvent
            {
                Info = infos,
                Killer = killer,
                Player = player
            };

            PlayerDeathEvent.Invoke(ev);
        }

        //PlayerHurtEvent
        public delegate void OnPlayerHurt(ref PlayerHurtEvent ev);
        public static event OnPlayerHurt PlayerHurtEvent;

        internal static void InvokePlayerHurtEvent(Player player, Player attacker,
            ref PlayerStats.HitInfo info)
        {
            if (PlayerHurtEvent == null) return;

            var ev = new PlayerHurtEvent
            {
                Player = player,
                Attacker = attacker,
                Info = info
            };

            PlayerHurtEvent.Invoke(ref ev);

            info = ev.Info;
        }
        
        //PlayerCuffedEvent
        public delegate void OnPlayerCuffed(ref PlayerCuffedEvent ev);
        public static event OnPlayerCuffed PlayerCuffedEvent;

        internal static void InvokePlayerCuffedEvent(Player cuffed, Player target, ref bool allow)
        {
            if (PlayerCuffedEvent == null) return;

            var ev = new PlayerCuffedEvent
            {
                Cuffer = cuffed,
                Target = target,
                Allow = allow
            };

            PlayerCuffedEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        //PlayerEscapeEvent
        public delegate void OnPlayerEscape(ref PlayerEscapeEvent ev);
        public static event OnPlayerEscape PlayerEscapeEvent;

        internal static void InvokePlayerEscapeEvent(Player player, ref bool allow, ref RoleType spawnRole,
            RoleType cuffedRole, bool isCuffed)
        {
            if (PlayerEscapeEvent == null) return;

            var ev = new PlayerEscapeEvent
            {
                Player = player,
                Allow = allow,
                SpawnRole = spawnRole,
                CufferRole = cuffedRole,
                IsCuffed = isCuffed
            };

            PlayerEscapeEvent.Invoke(ref ev);

            allow = ev.Allow;
            spawnRole = ev.SpawnRole;
        }

        //SyncDataEvent
        public delegate void OnSyncDataEvent(ref SyncDataEvent ev);
        public static event OnSyncDataEvent SyncDataEvent;

        internal static void InvokeSyncDataEvent(Player player, ref bool allow, ref Vector2 speed, int state)
        {
            if (SyncDataEvent == null) return;

            var ev = new SyncDataEvent
            {
                Allow = allow,
                Player = player,
                Speed = speed,
                State = state
            };

            SyncDataEvent.Invoke(ref ev);

            allow = ev.Allow;
            speed = ev.Speed;
        }

        //PlayerReloadEvent
        public delegate void OnPlayerReload(ref PlayerReloadEvent ev);
        public static event OnPlayerReload PlayerReloadEvent;

        internal static void InvokePlayerReloadEvent(Player player, ref bool allow, Inventory.SyncItemInfo syncItem)
        {
            if (PlayerReloadEvent == null) return;

            var ev = new PlayerReloadEvent
            {
                Player = player,
                Allow = allow,
                InventorySlot = syncItem
            };

            PlayerReloadEvent.Invoke(ref ev);

            allow = ev.Allow;
        }
        
        //FemurEnterEvent
        public delegate void OnFemurEnter(ref FemurEnterEvent ev);
        public static event OnFemurEnter FemurEnterEvent;

        internal static void InvokeFemurEnterEvent(Player player,ref bool allow,ref bool closeFemur)
        {
            if (FemurEnterEvent == null) return;

            var ev = new FemurEnterEvent
            {
                Player = player,
                Allow = allow,
                CloseFemur = closeFemur
            };

            FemurEnterEvent.Invoke(ref ev);

            allow = ev.Allow;
            closeFemur = ev.CloseFemur;
        }
        //DroppedItemEvent
        public delegate void OnDropItem(ref DropItemEvent ev);
        public static event OnDropItem DropItemEvent;

        internal static void InvokeDropItem(Player player, ref Inventory.SyncItemInfo item, ref bool allow)
        {
            if (DropItemEvent == null) return;
            
            var ev = new DropItemEvent
            {
                Player = player,
                Item = item,
                Allow = allow
            };
            
            DropItemEvent.Invoke(ref ev);

            allow = ev.Allow;
            item = ev.Item;
        }

        public delegate void OnLoadComponents(LoadComponentsEvent ev);
        public static event OnLoadComponents LoadComponentsEvent;
        internal static void InvokeLoadComponents(GameObject player) => LoadComponentsEvent?.Invoke(new LoadComponentsEvent { Player = player });

        public delegate void OnGeneratorInserted(ref GeneratorEvent ev);
        public static event OnGeneratorInserted GeneratorInsertedEvent;
        internal static void InvokeGeneratorInserted(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorInsertedEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorInsertedEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnGeneratorEjected(ref GeneratorEvent ev);
        public static event OnGeneratorEjected GeneratorEjectedEvent;
        internal static void InvokeGeneratorEjected(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorEjectedEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorEjectedEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnGeneratorUnlock(ref GeneratorEvent ev);
        public static event OnGeneratorUnlock GeneratorUnlockEvent;
        internal static void InvokeGeneratorUnlock(Player player,Generator079 generator, ref bool allow)
        {
            if (GeneratorUnlockEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorUnlockEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnGeneratorOpen(ref GeneratorEvent ev);
        public static event OnGeneratorOpen GeneratorOpenEvent;
        internal static void InvokeGeneratorOpen(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorOpenEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorOpenEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnGeneratorClose(ref GeneratorEvent ev);
        public static event OnGeneratorClose GeneratorCloseEvent;
        internal static void InvokeGeneratorClose(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorCloseEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorCloseEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnUseItem(ref UseItemEvent ev);
        public static event OnUseItem UseItemEvent;
        internal static void InvokeUseItemEvent(Player player,ItemType item, out bool allow)
        {
            allow = true;
            if (UseItemEvent == null) return;

            var ev = new UseItemEvent
            {
                Player = player,
                Item = item,
                Allow = true
            };

            UseItemEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnPickupItem(ref PickupItemEvent ev);
        public static event OnPickupItem PickupItemEvent;
        internal static void InvokePickupItemEvent(Player player, Pickup pickup, ref bool allow)
        {
            if (PickupItemEvent == null) return;

            var ev = new PickupItemEvent
            {
                Allow = allow,
                Pickup = pickup,
                Player = player
            };

            PickupItemEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnSetPlayerClass(ref PlayerSetClassEvent ev);

        public static event OnSetPlayerClass PlayerSetClassEvent;

        internal static void InvokePlayerSetClassEvent(Player player, ref RoleType type, ref List<ItemType> items)
        {
            var ev = new PlayerSetClassEvent
            {
                Items = items,
                Player = player,
                Role = type
            };
            
            PlayerSetClassEvent?.Invoke(ref ev);

            items = ev.Items;
            type = ev.Role;
        }


        public delegate void OnPlayerSetGroup(ref PlayerSetGroupEvent ev);
        public static event OnPlayerSetGroup PlayerSetGroupEvent;
        internal static void InvokePlayerSetGroupEvent(Player player, bool byAdmin, ref UserGroup group, ref bool ovr, ref bool disp, out bool allow)
        {
            allow = true;
            if (PlayerSetGroupEvent == null) return;

            var ev = new PlayerSetGroupEvent
            {
                Player = player,
                ByAdmin = byAdmin,
                Group = group,
                RaAcces = ovr,
                Showtag = disp,
                Allow = true
            };

            PlayerSetGroupEvent.Invoke(ref ev);

            group = ev.Group;
            ovr = ev.RaAcces;
            disp = ev.Showtag;
            allow = ev.Allow;
        }
    }
}