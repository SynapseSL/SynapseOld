using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        //Scp049RecallEvent
        public delegate void OnScp049Recall(ref Scp049RecallClass ev);
        /// <summary>A Event which is activated when Scp049 Recalls a Player</summary>
        public static event OnScp049Recall Scp049RecallEvent;
        internal static void InvokeScp049RecallEvent(ReferenceHub player, ref Ragdoll ragdoll, ref ReferenceHub target, ref bool allow, ref RoleType role, ref float lives)
        {
            if (Scp049RecallEvent == null) return;

            var ev = new Scp049RecallClass()
            {
                Allow = allow,
                Ragdoll = ragdoll,
                Target = target,
                RespawnRole = role,
                TargetHealth = lives,
                Player = player
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