using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Events.Patches
{
    public static class PlayerLeavePatch
    {
        public static bool Prefix(ReferenceHub __instance)
        {
            try
            {
                Events.InvokePlayerLeaveEvent(__instance);
            }
            catch (Exception e)
            {
                Log.Error($"Player Leave Event Error: {e}");
            }

            return true;
        }
    }
}
