using System;
using Harmony;

namespace Synapse.Events.Patches
{
	
	/// <summary>
	/// The WaitingForPlayers Patch is a special one.
	///
	/// This solution is currently the only way to check if a server is finished with initialisation or not.
	/// Since the "Init"-Method (where the Server States that it is indeed "ready for players" is an IEnumerator, Harmony didn't really have a way of patching
	/// the IEnumerator. Until Northwood does a better Handling of this, this is here to stay.
	/// </summary>
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class WaitingForPlayersPatch
    {
		public static void Prefix(ref string q)
		{
			try
			{
				if (q == "Waiting for players...") Events.InvokeWaitingForPlayers();
			}
			catch (Exception e)
			{
				Log.Error($"WaitingForPlayersEvent error: {e}");
			}
		}
	}
}
