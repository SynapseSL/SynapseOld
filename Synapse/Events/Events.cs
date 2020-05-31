using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Events
{
    public class Events
    {
        //JoinEvent
        public delegate void Playerjoin(ref PlayerJoinEvent ev);
        /// <summary>A Event which is activated when a User Joins the Server</summary>
        /// <remarks>It need to hook ref PlayerJoinEvent ev</remarks>
        public static event Playerjoin PlayerjoinEvent;
        public static void InvokePlayerjoinEvent(ReferenceHub player,ref string nick)
        {
            if (PlayerjoinEvent == null) return;
            var ev = new PlayerJoinEvent(player)
            {
                Nick = nick,
            };

            PlayerjoinEvent.Invoke(ref ev);

            nick = ev.Nick;
        }
    }
}
