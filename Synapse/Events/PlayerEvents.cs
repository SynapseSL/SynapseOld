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
        public delegate void OnPlayerJoin(PlayerJoinEvent ev);
        public static event OnPlayerJoin PlayerJoinEvent;

        internal static void InvokePlayerJoinEvent(Player player, ref string nick)
        {
            if (PlayerJoinEvent == null) return;
            var ev = new PlayerJoinEvent
            {
                Player = player,
                Nick = nick
            };

            PlayerJoinEvent.Invoke(ev);

            nick = ev.Nick;
        }

        /// <summary>A Event which is activated when a user press any voice HotKey</summary>
        public delegate void OnSpeak(PlayerSpeakEvent ev);
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

            SpeakEvent.Invoke(ev);

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
        public delegate void OnPlayerBanEvent(PlayerBanEvent ev);
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

            PlayerBanEvent.Invoke(ev);

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
        public delegate void OnPlayerHurt(PlayerHurtEvent ev);
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

            PlayerHurtEvent.Invoke(ev);

            info = ev.Info;
        }
        
        //PlayerCuffedEvent
        public delegate void OnPlayerCuffed(PlayerCuffedEvent ev);
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

            PlayerCuffedEvent.Invoke(ev);

            allow = ev.Allow;
        }

        //PlayerEscapeEvent
        public delegate void OnPlayerEscape(PlayerEscapeEvent ev);
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

            PlayerEscapeEvent.Invoke(ev);

            allow = ev.Allow;
            spawnRole = ev.SpawnRole;
        }

        //SyncDataEvent
        public delegate void OnSyncDataEvent(SyncDataEvent ev);
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

            SyncDataEvent.Invoke(ev);

            allow = ev.Allow;
            speed = ev.Speed;
        }

        //PlayerReloadEvent
        public delegate void OnPlayerReload(PlayerReloadEvent ev);
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

            PlayerReloadEvent.Invoke(ev);

            allow = ev.Allow;
        }
        
        //FemurEnterEvent
        public delegate void OnFemurEnter(FemurEnterEvent ev);
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

            FemurEnterEvent.Invoke(ev);

            allow = ev.Allow;
            closeFemur = ev.CloseFemur;
        }
        //DroppedItemEvent
        public delegate void OnDropItem(DropItemEvent ev);
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
            
            DropItemEvent.Invoke(ev);

            allow = ev.Allow;
            item = ev.Item;
        }

        public delegate void OnLoadComponents(LoadComponentsEvent ev);
        public static event OnLoadComponents LoadComponentsEvent;
        internal static void InvokeLoadComponents(GameObject player) => LoadComponentsEvent?.Invoke(new LoadComponentsEvent { Player = player });

        public delegate void OnGenerator(GeneratorEvent ev);
        public static event OnGenerator GeneratorInsertedEvent;
        internal static void InvokeGeneratorInserted(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorInsertedEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorInsertedEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public static event OnGenerator GeneratorEjectedEvent;
        internal static void InvokeGeneratorEjected(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorEjectedEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorEjectedEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public static event OnGenerator GeneratorUnlockEvent;
        internal static void InvokeGeneratorUnlock(Player player,Generator079 generator, ref bool allow)
        {
            if (GeneratorUnlockEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorUnlockEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public static event OnGenerator GeneratorOpenEvent;
        internal static void InvokeGeneratorOpen(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorOpenEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorOpenEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public static event OnGenerator GeneratorCloseEvent;
        internal static void InvokeGeneratorClose(Player player, Generator079 generator, ref bool allow)
        {
            if (GeneratorCloseEvent == null) return;

            var ev = new GeneratorEvent
            {
                Allow = allow,
                Generator = generator,
                Player = player
            };

            GeneratorCloseEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public delegate void OnUseItem(UseItemEvent ev);
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

            UseItemEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public delegate void OnPickupItem(PickupItemEvent ev);
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

            PickupItemEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public delegate void OnSetPlayerClass(PlayerSetClassEvent ev);

        public static event OnSetPlayerClass PlayerSetClassEvent;

        internal static void InvokePlayerSetClassEvent(Player player, ref RoleType type, ref List<ItemType> items)
        {
            var ev = new PlayerSetClassEvent
            {
                Items = items,
                Player = player,
                Role = type
            };
            
            PlayerSetClassEvent?.Invoke(ev);

            items = ev.Items;
            type = ev.Role;
        }


        public delegate void OnPlayerTag(PlayerTagEvent ev);
        public static event OnPlayerTag PlayerTagEvent;
        internal static void InvokePlayerTagEvent(Player player, bool show,out bool allow)
        {
            allow = true;
            if (PlayerTagEvent == null) return;

            var ev = new PlayerTagEvent
            {
                Player = player,
                ShowTag = show,
                Allow = true
            };

            PlayerTagEvent.Invoke(ev);

            allow = ev.Allow;
        }


        public delegate void OnKeyPress(KeyPressEvent ev);
        public static event OnKeyPress KeyPressEvent;
        internal static void InvokeKeyPressEvent(Player player, KeyCode key)
        {
            if (KeyPressEvent == null) return;

            var ev = new KeyPressEvent()
            {
                Player = player,
                Key = key
            };

            KeyPressEvent.Invoke(ev);
        }

        public delegate void OnPlayerHeal(PlayerHealEvent ev);

        public static event OnPlayerHeal PlayerHealEvent;
        internal static void InvokePlayerHealEvent(Player player, ref float amount, out bool allow)
        {
            allow = true;
            if (PlayerHealEvent == null) return;
            
            var ev = new PlayerHealEvent
            {
                Player = player,
                Amount = amount,
                Allow = allow
            };
            
            PlayerHealEvent.Invoke(ev);

            allow = ev.Allow;
            amount = ev.Amount;

        }
    }
}