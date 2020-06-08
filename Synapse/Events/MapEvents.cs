using System.Diagnostics.CodeAnalysis;
using Synapse.Events.Classes;
using UnityEngine;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        // DoorInteractEvent
        public delegate void OnDoorInteract(ref DoorInteractEvent ev);

        public static event OnDoorInteract DoorInteractEvent;

        internal static void InvokeDoorInteraction(ReferenceHub player, Door door, ref bool allow)
        {
            if (DoorInteractEvent == null) return;

            var ev = new DoorInteractEvent
            {
                Player = player,
                Allow = allow,
                Door = door
            };

            DoorInteractEvent.Invoke(ref ev);

            allow = ev.Allow;
        }
        //TeslaTriggerEvent
        public delegate void OnTriggerTesla(ref TeslaTriggerEvent ev);
        public static event OnTriggerTesla TeslaTriggerEvent;

        internal static void InvokeTeslaTrigger(GameObject player, bool inRange, ref bool activated)
        {
            if (TeslaTriggerEvent == null) return;

            var ev = new TeslaTriggerEvent
            {
                Player = player.GetComponent<ReferenceHub>(),
                IsHurtRange = inRange,
                Triggerable = activated
            };

            TeslaTriggerEvent?.Invoke(ref ev);

            activated = ev.Triggerable;
        }

        public delegate void OnWarheadDetonation();
        public static event OnWarheadDetonation WarheadDetonationEvent;

        internal static void InvokeWarheadDetonation() => WarheadDetonationEvent?.Invoke();
    }
}