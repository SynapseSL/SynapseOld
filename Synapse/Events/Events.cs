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

        //Speak Event
        public delegate void Speak(ref SpeakEvent ev);
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
    }
}
