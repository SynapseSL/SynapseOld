using System.Collections.Generic;
using System.Linq;
using Harmony;
using Mirror;
using NorthwoodLib.Pools;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.SetPlayersClass))]
    public class PlayerSetClassPatch
    {
        public static bool Prefix(CharacterClassManager __instance, RoleType classid, GameObject ply,
            bool lite = false, bool escape = false)
        {
            if (!NetworkServer.active) return false;
            if (!ply.GetPlayer().ClassManager.IsVerified) return false;

            var inventory =
                lite ? new List<ItemType>(0) : __instance.Classes.SafeGet(classid).startItems.ToList();
            Events.InvokePlayerSetClassEvent(ply.GetPlayer(), ref classid, ref inventory);
            ply.GetPlayer().ClassManager.SetClassIDAdv(classid, lite, escape);
            ply.GetPlayer().Health = __instance.Classes.SafeGet(classid).maxHP;

            if (lite) return false;
            var inv = ply.GetPlayer().Inventory;
            var list = ListPool<Inventory.SyncItemInfo>.Shared.Rent();

            if (escape && __instance.KeepItemsAfterEscaping)
            {
                list.AddRange(inv.items);
            }
            
            inv.items.Clear();
            foreach (var id in inventory)
            {
                inv.AddNewItem(id);
            }

            if (!escape || !__instance.KeepItemsAfterEscaping) return false;
            foreach (var syncItemInfo in list)
            {
                if (__instance.PutItemsInInvAfterEscaping)
                {
                    var itemById = inv.GetItemByID(syncItemInfo.id);
                    var flag = false;
                    var categories = __instance._search.categories;
                    var i = 0;

                    while (i < categories.Length)
                    {
                        var invCat = categories[i];
                        if (invCat.itemType == itemById.itemCategory && itemById.itemCategory != ItemCategory.None)
                        {
                            var num = inv.items.Count(syncItemInfo2 => inv.GetItemByID(syncItemInfo2.id).itemCategory == itemById.itemCategory);

                            if (num >= invCat.maxItems)
                            {
                                flag = true;
                            }

                            break;
                        }
                        i++;
                    }

                    if (inv.items.Count >= 8 || flag)
                    {
                        inv.SetPickup(syncItemInfo.id, syncItemInfo.durability, __instance._pms.RealModelPosition,
                            Quaternion.Euler(__instance._pms.Rotations.x, __instance._pms.Rotations.y, 0f),
                            syncItemInfo.modSight, syncItemInfo.modBarrel, syncItemInfo.modOther);
                    }
                    else
                    {
                        inv.AddNewItem(syncItemInfo.id, syncItemInfo.durability, syncItemInfo.modSight, syncItemInfo.modBarrel, syncItemInfo.modOther);
                    }
                }
                else
                {
                    inv.SetPickup(syncItemInfo.id, syncItemInfo.durability, __instance._pms.RealModelPosition,
                        Quaternion.Euler(__instance._pms.Rotations.x, __instance._pms.Rotations.y, 0f),
                        syncItemInfo.modSight, syncItemInfo.modBarrel, syncItemInfo.modOther);
                }
            }

            return false;
        }
    }
}