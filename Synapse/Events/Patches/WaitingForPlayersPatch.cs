using System;
using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class WaitingForPlayersPatch
    {
		public static void Prefix(ref string q)
		{
			try
			{
				if (q == "Waiting for players..") Events.InvokeWaitingforPlayers();
			}
			catch (Exception e)
			{
				Log.Error($"WaitingForPlayersEvent error: {e}");
			}
		}
	}
}
