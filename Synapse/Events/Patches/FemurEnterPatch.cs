using System;
using Harmony;
using Mirror;
using Synapse.Api;
using Synapse.Configs;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.AllowContain))]
    public class FemurEnterPatch
    {
		public static int FemurBrokePeople;

        public static bool Prefix(CharacterClassManager __instance)
        {
            try
            {
				if (!NetworkServer.active) return false;
				if (!NonFacilityCompatibility.currentSceneSettings.enableStandardGamplayItems) return false;

				foreach (var gameObject in PlayerManager.players)
				{
					if (!(Vector3.Distance(gameObject.transform.position, __instance._lureSpj.transform.position) <
					      1.97f)) continue;
					var component = gameObject.GetComponent<CharacterClassManager>();
					var component2 = gameObject.GetComponent<PlayerStats>();
					if (component.CurClass == RoleType.Spectator || component.GodMode) continue;
					var allow = component.CurRole.team != Team.SCP;

					var closeFemur = FemurBrokePeople + 1 >= SynapseConfigs.RequiredForFemur;
					var player = __instance.GetPlayer();

					Events.InvokeFemurEnterEvent(player, ref allow, ref closeFemur);

					if (!allow) return false;
					component2.HurtPlayer(new PlayerStats.HitInfo(10000f, "WORLD", DamageTypes.Lure, 0), gameObject);
					FemurBrokePeople++;
					if (closeFemur) __instance._lureSpj.SetState(true);
				}

				return false;
            }
            catch (Exception e)
            {
                Log.Error($"FemurEnterEvent Error: {e}");
                return true;
            }
        }
    }
}
