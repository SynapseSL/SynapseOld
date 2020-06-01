using Assets._Scripts.Dissonance;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        
        //PlayerDieEvent
        public delegate void OnPlayerDeath(ref PlayerDeathClass ev);
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

            PlayerDeathEvent.Invoke(ref ev);

            infos = ev.Info;
        }

        //PlaerHurtEvent
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
        
    }
}