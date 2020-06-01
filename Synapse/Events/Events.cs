using Assets._Scripts.Dissonance;

namespace Synapse.Events
{
    public static class Events
    {
        //JoinEvent
        public delegate void PlayerJoin(ref PlayerJoinEvent ev);
        /// <summary>A Event which is activated when a User Joins the Server</summary>
        /// <remarks>It need to hook ref PlayerJoinEvent ev</remarks>
        public static event PlayerJoin PlayerJoinEvent;
        public static void InvokePlayerJoinEvent(ReferenceHub player,ref string nick)
        {
            if (PlayerJoinEvent == null) return;
            var ev = new PlayerJoinEvent(player)
            {
                Nick = nick,
            };

            PlayerJoinEvent.Invoke(ref ev);

            nick = ev.Nick;
        }


        //RemoteCommandEvent
        public delegate void RemoteCommand(ref RemoteCommandEvent ev);
        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        /// <remarks>It need to hook ref RemoteCommandEvent ev</remarks>
        public static event RemoteCommand RemoteCommandEvent;
        public static void InvokeRemoteCommandEvent(CommandSender sender,string command,ref bool allow)
        {
            if (RemoteCommandEvent == null) return;

            var ev = new RemoteCommandEvent()
            {
                Allow = allow,
                Sender = sender,
                Command = command,
            };

            RemoteCommandEvent.Invoke(ref ev);

            allow = ev.Allow;
        }


        //ConsoleCommandEvent
        public delegate void ConsoleCommand(ref ConsoleCommandEvent ev);
        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        public static event ConsoleCommand ConsoleCommandEvent;
        public static void InvokeConsoleCommandEvent(ReferenceHub player,string command,out string color,out string returning)
        {
            color = "red";
            returning = "";
            if (ConsoleCommandEvent == null) return;

            var ev = new ConsoleCommandEvent()
            {
                Command = command,
                Player = player,
            };

            ConsoleCommandEvent.Invoke(ref ev);

            color = ev.Color;
            returning = ev.ReturnMessage;
        }


        //SpeakEvent
        public delegate void Speak(ref SpeakEvent ev);
        /// <summary>A Event which is activated when a user press any voice hotkey</summary>
        public static event Speak SpeakEvent;
        public static void InvokeSpeakEvent(DissonanceUserSetup dissonance, ref bool intercom, ref bool radio, ref bool scp939, ref bool scpchat, ref bool spectator)
        {
            if (SpeakEvent == null) return;

            var ev = new SpeakEvent()
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


        //Scp049RecallEvent
        public delegate void Scp049Recall(ref Scp049RecallEvent ev);
        /// <summary>A Event which is activated when Scp049 Recalls a Player</summary>
        public static event Scp049Recall Scp049RecallEvent;
        public static void InvokeScp049RecallEvent(ReferenceHub player, ref Ragdoll ragdoll, ref ReferenceHub target, ref bool allow, ref RoleType role, ref float lives)
        {
            if (Scp049RecallEvent == null) return;

            Scp049RecallEvent ev = new Scp049RecallEvent()
            {
                Allow = allow,
                Ragdoll = ragdoll,
                Target = target,
                RespawnRole = role,
                TargetHealth = lives,
                Player = player
            };

            Scp049RecallEvent.Invoke(ref ev);

            ragdoll = ev.Ragdoll;
            target = ev.Target;
            role = ev.RespawnRole;
            lives = ev.TargetHealth;
            allow = ev.Allow;
        }


        //PlayerLeaveEvent
        public delegate void PlayerLeave(PlayerLeaveEvent ev);
        /// <summary>
        /// A Event which is activated when a User leave the server
        /// </summary>
        public static event PlayerLeave PlayerLeaveEvent;
        public static void InvokePlayerLeaveEvent(ReferenceHub player)
        {
            if (PlayerLeaveEvent == null) return;

            var ev = new PlayerLeaveEvent()
            {
                Player = player,
            };
            PlayerLeaveEvent.Invoke(ev);
        }
        
        // RoundStartEvent
        public delegate void OnRoundStart();
        public static event OnRoundStart RoundStartEvent;

        public static void InvokeRoundStart() => RoundStartEvent?.Invoke();

        // PlayerBanEvent
        public delegate void OnPlayerBanEvent(ref PlayerBanEvent ev);

        public static event OnPlayerBanEvent PlayerBanEvent;

        public static void InvokePlayerBanEvent(ReferenceHub player, string userId, int duration, ref bool allow,
            string reason, ReferenceHub issuer)
        {
            if (PlayerBanEvent == null) return;
            
            var ev = new PlayerBanEvent()
            {
                Issuer = issuer,
                Duration = duration,
                Reason = reason,
                BannedPlayer =  player,
                UserId = userId
            };
            
            PlayerBanEvent.Invoke(ref ev);

            allow = ev.Allowed;
        }
    }
}
