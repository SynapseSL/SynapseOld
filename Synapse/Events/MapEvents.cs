using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        // DoorInteractEvent
        public delegate void OnDoorInteract(ref DoorInteractClass ev);
        public static event OnDoorInteract DoorInteractEvent;
        internal static void InvokeDoorInteraction(ReferenceHub player, Door door, ref bool allow)
        {
            if (DoorInteractEvent == null) return;

            var ev = new DoorInteractClass()
            {
                Player = player,
                Allow = allow,
                Door = door
            };
            
            DoorInteractEvent.Invoke(ref ev);

            allow = ev.Allow;
        }
    }
}