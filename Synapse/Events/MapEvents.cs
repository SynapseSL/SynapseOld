using System.Diagnostics.CodeAnalysis;
using Synapse.Events.Classes;
using UnityEngine;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        // DoorInteractEvent
        public delegate void OnDoorInteract(ref DoorInteractClass ev);

        public static event OnDoorInteract DoorInteractEvent;

        internal static void InvokeDoorInteraction(ReferenceHub player, Door door, ref bool allow)
        {
            if (DoorInteractEvent == null) return;

            var ev = new DoorInteractClass
            {
                Player = player,
                Allow = allow,
                Door = door
            };

            DoorInteractEvent.Invoke(ref ev);

            allow = ev.Allow;
        }
        //TeslaTriggerEvent
        public delegate void OnTriggerTesla(ref TeslaTriggerClass ev);
        public static event OnTriggerTesla TeslaTriggerEvent;

        internal static void InvokeTeslaTrigger(GameObject player, bool inRange, ref bool activated)
        {
            if (TeslaTriggerEvent == null) return;

            var ev = new TeslaTriggerClass
            {
                Player = player.GetComponent<ReferenceHub>(),
                IsHurtRange = inRange,
                Triggerable = activated
            };

            TeslaTriggerEvent?.Invoke(ref ev);

            activated = ev.Triggerable;
        }
    }
}