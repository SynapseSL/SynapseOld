using Assets._Scripts.Dissonance;
using System;
using Harmony;

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
				bool intercom = __instance.IntercomAsHuman;
				bool radio = __instance.RadioAsHuman;
				bool scp939 = __instance.MimicAs939;
				bool scpchat = __instance.SCPChat;
				bool spectator = __instance.SpectatorChat;

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
