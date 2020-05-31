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
        public delegate void Playerjoin(PlayerJoinEvent ev);
        /// <summary>A Event which is activated when a User Joins the Server</summary>
        public static event Playerjoin PlayerjoinEvent;
        public static void InvokePlayerjoinEvent(ReferenceHub player)
        {
            if (PlayerjoinEvent == null) return;
            var ev = new PlayerJoinEvent()
            {
                Player = player,
            };

            PlayerjoinEvent.Invoke(ev);
        }
    }
}
