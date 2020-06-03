﻿using System.Runtime.Remoting.Messaging;
using Assets._Scripts.Dissonance;
using Synapse.Events.Classes;
using UnityEngine;

namespace Synapse.Events
{
    public static partial class Events
    {
        //JoinEvent
        public delegate void OnPlayerJoin(ref PlayerJoinClass ev);
        /// <summary>A Event which is activated when a User Joins the Server</summary>
        /// <remarks>It need to hook ref PlayerJoinEvent ev</remarks>
        public static event OnPlayerJoin PlayerJoinEvent;
        internal static void InvokePlayerJoinEvent(ReferenceHub player, ref string nick)
        {
            if (PlayerJoinEvent == null) return;
            var ev = new PlayerJoinClass(player)
            {
                Nick = nick,
            };

            PlayerJoinEvent.Invoke(ref ev);

            nick = ev.Nick;
        }

        //SpeakEvent
        public delegate void OnSpeak(ref SpeakEventClass ev);
        /// <summary>A Event which is activated when a user press any voice hotkey</summary>
        public static event OnSpeak SpeakEvent;
        internal static void InvokeSpeakEvent(DissonanceUserSetup dissonance, ref bool intercom, ref bool radio, ref bool scp939, ref bool scpchat, ref bool spectator)
        {
            if (SpeakEvent == null) return;

            var ev = new SpeakEventClass()
            {
                IntercomTalk = intercom,
                RadioTalk = radio,
                Scp939Talk = scp939,
                ScpChat = scpchat,
                SpectatorChat = spectator,
                DissonanceUserSetup = dissonance,
                Player = dissonance.gameObject.GetComponent<ReferenceHub>(),
            };

            SpeakEvent.Invoke(ref ev);

            intercom = ev.IntercomTalk;
            radio = ev.RadioTalk;
            scp939 = ev.Scp939Talk;
            scpchat = ev.ScpChat;
            spectator = ev.SpectatorChat;
        }

        //PlayerLeaveEvent
        public delegate void OnPlayerLeave(PlayerLeaveClass ev);
        /// <summary>
        /// A Event which is activated when a User leave the server
        /// </summary>
        public static event OnPlayerLeave PlayerLeaveEvent;
        internal static void InvokePlayerLeaveEvent(ReferenceHub player)
        {
            if (PlayerLeaveEvent == null) return;

            var ev = new PlayerLeaveClass()
            {
                Player = player,
            };
            PlayerLeaveEvent.Invoke(ev);
        }
        
        // PlayerBanEvent
        public delegate void OnPlayerBanEvent(ref PlayerBanClass ev);
        public static event OnPlayerBanEvent PlayerBanEvent;
        internal static void InvokePlayerBanEvent(ReferenceHub player, string userId, int duration, ref bool allow,
            string reason, ReferenceHub issuer)
        {
            if (PlayerBanEvent == null) return;

            var ev = new PlayerBanClass()
            {
                Issuer = issuer,
                Duration = duration,
                Reason = reason,
                BannedPlayer = player,
                UserId = userId
            };

            PlayerBanEvent.Invoke(ref ev);

            allow = ev.Allowed;
        }

        //PlayerDieEvent
        public delegate void OnPlayerDeath(PlayerDeathClass ev);
        public static event OnPlayerDeath PlayerDeathEvent;
        internal static void InvokePlayerDieEvent(ReferenceHub player, ReferenceHub killer, PlayerStats.HitInfo infos)
        {
            if (PlayerDeathEvent == null) return;

            var ev = new PlayerDeathClass()
            {
                Info = infos,
                Killer = killer,
                Player = player,
            };

            PlayerDeathEvent.Invoke(ev);
        }

        //PlayerHurtEvent
        public delegate void OnPlayerHurt(ref PlayerHurtClass ev);
        public static event OnPlayerHurt PlayerHurtEvent;
        internal static void InvokePlayerHurtEvent(ReferenceHub player, ReferenceHub attacker, ref PlayerStats.HitInfo info)
        {
            if (PlayerHurtEvent == null) return;

            var ev = new PlayerHurtClass()
            {
                Player = player,
                Attacker = attacker,
                Info = info,
            };

            PlayerHurtEvent.Invoke(ref ev);

            info = ev.Info;
        }

        //PlayerCuffedEvent
        public delegate void OnPlayerCuffed(ref PlayerCuffedClass ev);
        public static event OnPlayerCuffed PlayerCuffedEvent;
        internal static void InvokePlayerCuffedEvent(ReferenceHub cuffer, ReferenceHub target, ref bool allow)
        {
            if (PlayerCuffedEvent == null) return;

            var ev = new PlayerCuffedClass()
            {
                Cuffer = cuffer,
                Target = target,
                Allow = allow,
            };

            PlayerCuffedEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        //PlayerEscapeEvent
        public delegate void OnPlayerEscape(ref PlayerEscapeClass ev);
        public static event OnPlayerEscape PlayerEscapeEvent;
        internal static void InvokePlayerEscapeEvent(ReferenceHub player, ref bool allow,ref RoleType spawnrole, RoleType cufferrole, bool iscuffed)
        {
            if (PlayerEscapeEvent == null) return;

            var ev = new PlayerEscapeClass()
            {
                Player = player,
                Allow = allow,
                SpawnRole = spawnrole,
                CufferRole = cufferrole,
                IsCuffed = iscuffed,
            };

            PlayerEscapeEvent.Invoke(ref ev);

            allow = ev.Allow;
            spawnrole = ev.SpawnRole;
        }
        
        //SyncDataEvent
        public delegate void OnSyncDataEvent(ref SyncDataClass ev);
        public static event OnSyncDataEvent SyncDataEvent;

        internal static void InvokeSyncDataEvent(GameObject player, ref bool allow, ref Vector2 speed, int state)
        {
            if (SyncDataEvent == null) return;
            
            var ev = new SyncDataClass()
            {
                Allow = allow,
                Player = player.GetComponent<ReferenceHub>(),
                Speed = speed,
                State = state
            };
            
            SyncDataEvent.Invoke(ref ev);

            allow = ev.Allow;
            speed = ev.Speed;
        }
    }
}
