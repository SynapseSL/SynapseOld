using System;
using Harmony;
using UnityEngine;
using Console = GameCore.Console;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Console), nameof(Console.AddLog), typeof(string), typeof(Color), typeof(bool))]
    public class RoundEndPatch
    {
        public static void Prefix(string text)
		{
			if (!text.StartsWith("Round finished! Anomalies: "))
				return;

			try
			{
				Events.InvokeRoundEndEvent();
			}
			catch (Exception exception)
			{
				Log.Error($"RoundEndEvent error: {exception}");
			}
		}
	}
}