using System;
using Assets._Scripts.Dissonance;
using Harmony;
using Synapse.Api;
using Synapse.Config;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(DissonanceUserSetup), nameof(DissonanceUserSetup.CallCmdAltIsActive))]
    public class SpeakPatch
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(DissonanceUserSetup __instance, bool value)
        {
            try
            {
                var intercom = __instance.IntercomAsHuman;
                var radio = __instance.RadioAsHuman;
                var scp939 = __instance.MimicAs939;
                var scpchat = __instance.SCPChat;
                var spectator = __instance.SpectatorChat;

                scp939 = SynapseConfigs.SpeakingScps.Contains((int)__instance.GetPlayer().Role);

                Events.InvokeSpeakEvent(__instance, ref intercom, ref radio, ref scp939, ref scpchat, ref spectator);

                __instance.SCPChat = scpchat;
                __instance.SpectatorChat = spectator;
                __instance.IntercomAsHuman = intercom;

                if (scp939) __instance.MimicAs939 = value;
                if (radio) __instance.RadioAsHuman = value;

                return true;
            }
            catch (Exception e)
            {
                Log.Error($"SpeakEvent Error: {e}");
                return true;
            }
        }
    }
}