using Synapse.Events.Classes;
using Synapse.Api;

namespace Synapse.Events
{
    public static partial class Events
    {
        public delegate void OnScp106Containment(Scp106ContainmentEvent ev);
        public static event OnScp106Containment Scp106ContainmentEvent;
        internal static void InvokeScp106ContainmentEvent(Player player, ref bool allow)
        {
            if (Scp106ContainmentEvent == null) return;

            var ev = new Scp106ContainmentEvent
            {
                Player = player,
                Allow = allow
            };

            Scp106ContainmentEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public delegate void OnScp106CreatePortal(Scp106CreatePortalEvent ev);
        public static event OnScp106CreatePortal Scp106CreatePortalEvent;
        internal static void InvokeScp106CreatePortalEvent(Player player, ref bool allow)
        {
            var ev = new Scp106CreatePortalEvent
            {
                Allow = allow,
                Player = player
            };

            Scp106CreatePortalEvent?.Invoke(ev);

            allow = ev.Allow;
        }

        public delegate void OnPocketDimensionEnter(PocketDimensionEvent ev);
        public static event OnPocketDimensionEnter PocketDimensionEnterEvent;
        internal static void InvokePocketDimensionEnterEvent(Player player, ref bool allow)
        {
            if (PocketDimensionEnterEvent == null) return;

            var ev = new PocketDimensionEvent
            {
                Player = player,
                Allow = allow
            };

            PocketDimensionEnterEvent.Invoke(ev);

            allow = ev.Allow;
        }

        public delegate void OnPocketDimensionLeave(PocketDimensionLeave ev);
        public static event OnPocketDimensionLeave PocketDimensionLeaveEvent;
        internal static void InvokePocketDimensionLeave(Player player, ref PocketDimensionTeleport.PDTeleportType type, out bool allow)
        {
            allow = true;
            if (PocketDimensionLeaveEvent == null) return;

            var ev = new PocketDimensionLeave
            {
                Allow = true,
                Player = player,
                TeleportType = type
            };

            PocketDimensionLeaveEvent.Invoke(ev);

            allow = ev.Allow;
            type = ev.TeleportType;
        }
    }
}
