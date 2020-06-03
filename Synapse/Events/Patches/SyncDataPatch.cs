using System;
using Harmony;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(AnimationController), nameof(AnimationController.CallCmdSyncData))]
    public class SyncDataPatch
    {
        public static bool Prefix(AnimationController __instance, int state, Vector2 v2)
        {
            try
            {
                if (!__instance._mSyncRateLimit.CanExecute(false))
                {
                    return false;
                }

                var allow = true;

                Events.InvokeSyncDataEvent(__instance.gameObject, ref allow, ref v2, state);

                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"SyncDataPatchErr: {e}");
                return true;
            }
        }
    }
}