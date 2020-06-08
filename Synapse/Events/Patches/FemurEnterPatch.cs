using System;
using Harmony;
using Mirror;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.AllowContain))]
    public class FemurEnterPatch
    {
		public static int femurBrokePeople;

        public static bool Prefix(CharacterClassManager __instance)
        {
            try
            {
				if (!NetworkServer.active) return false;
				if (!NonFacilityCompatibility.currentSceneSettings.enableStandardGamplayItems) return false;

				foreach (GameObject gameObject in PlayerManager.players)
				{
					if (Vector3.Distance(gameObject.transform.position, __instance._lureSpj.transform.position) < 1.97f)
					{
						CharacterClassManager component = gameObject.GetComponent<CharacterClassManager>();
						PlayerStats component2 = gameObject.GetComponent<PlayerStats>();
						if (component.CurRole.team != Team.SCP && component.CurClass != RoleType.Spectator && !component.GodMode)
						{
							var allow = true;
							var CloseFemur = femurBrokePeople + 1 >= Configs.requiredforFemur;
							var player = __instance.GetComponent<ReferenceHub>();

							Events.InvokeFemurEnterEvent(player, ref allow, ref CloseFemur);

							if (!allow) return false;
							component2.HurtPlayer(new PlayerStats.HitInfo(10000f, "WORLD", DamageTypes.Lure, 0), gameObject);
							femurBrokePeople++;
							if (CloseFemur) __instance._lureSpj.SetState(true);
						}
					}
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
