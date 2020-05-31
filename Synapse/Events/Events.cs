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
    }
}
