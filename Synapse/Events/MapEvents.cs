using System.Diagnostics.CodeAnalysis;
using Synapse.Api;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        // DoorInteractEvent
        public delegate void OnDoorInteract(DoorInteractEvent ev);

        public static event OnDoorInteract DoorInteractEvent;

        internal static void InvokeDoorInteraction(Player player, Door door, ref bool allow)
        {
            if (DoorInteractEvent == null) return;

            var ev = new DoorInteractEvent
            {
                Player = player,
                Allow = allow,
                Door = door
            };

            DoorInteractEvent.Invoke(ev);

            allow = ev.Allow;
        }
        //TeslaTriggerEvent
        public delegate void OnTriggerTesla(TeslaTriggerEvent ev);
        public static event OnTriggerTesla TeslaTriggerEvent;

        internal static void InvokeTeslaTrigger(Player player, bool inRange, ref bool activated)
        {
            if (TeslaTriggerEvent == null) return;

            var ev = new TeslaTriggerEvent
            {
                Player = player,
                IsHurtRange = inRange,
                Triggerable = activated
            };

            TeslaTriggerEvent?.Invoke(ev);

            activated = ev.Triggerable;
        }

        public delegate void OnWarheadDetonation();
        public static event OnWarheadDetonation WarheadDetonationEvent;

        internal static void InvokeWarheadDetonation() => WarheadDetonationEvent?.Invoke();
    }
}