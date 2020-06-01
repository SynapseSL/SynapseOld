using Assets._Scripts.Dissonance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var ev = new RemoteCommandEvent(sender, command)
            {
                Allow = allow,
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

        //Speak Event
        public delegate void Speak(ref SpeakEvent ev);
        /// <summary>A Event which is activated when a user press any voice hotkey</summary>
        public static event Speak SpeakEvent;
        public static void InvokeSpeakEvent(DissonanceUserSetup dissonance, ref bool intercom, ref bool radio, ref bool scp939, ref bool scpchat, ref bool spectator)
        {
            if (SpeakEvent == null) return;

            SpeakEvent ev = new SpeakEvent(dissonance.gameObject.GetComponent<ReferenceHub>(), dissonance)
            {
                IntercomTalk = intercom,
                RadioTalk = radio,
                Scp939Talk = scp939,
                ScpChat = scpchat,
                SpectatorChat = spectator
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

            Scp049RecallEvent ev = new Scp049RecallEvent(player)
            {
                Allow = allow,
                Ragdoll = ragdoll,
                Target = target,
                RespawnRole = role,
                TargetHealth = lives,
            };

            Scp049RecallEvent.Invoke(ref ev);

            ragdoll = ev.Ragdoll;
            target = ev.Target;
            role = ev.RespawnRole;
            lives = ev.TargetHealth;
            allow = ev.Allow;
        }
    }
}
