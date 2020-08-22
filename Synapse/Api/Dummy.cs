﻿using Mirror;
using RemoteAdmin;
using System.Linq;
using UnityEngine;

namespace Synapse.Api
{
    public class Dummy
    {
        private ItemType helditem;
        private GameObject gameObject;

        /// <summary>
        /// Get / Set the Current Role of the Dummy
        /// </summary>
        public RoleType Role
        {
            get => gameObject.GetComponent<CharacterClassManager>().CurClass;
            set
            {
                Despawn();
                gameObject.GetComponent<CharacterClassManager>().CurClass = value;
                Spawn();
            }
        }

        /// <summary>
        /// Get / Set the Current Name of the Dummy
        /// </summary>
        public string Name
        {
            get => gameObject.GetComponent<NicknameSync>().Network_myNickSync;
            set => gameObject.GetComponent<NicknameSync>().Network_myNickSync = value;
        }


        /// <summary>
        /// Get / Set the Current Position of the Dummy
        /// </summary>
        public Vector3 Position
        {
            get => gameObject.transform.position;
            set
            {
                Despawn();
                gameObject.transform.position = value;
                Spawn();
            }
        }


        /// <summary>
        /// Get / Set the Current Item the Dummy is holding
        /// </summary>
        public ItemType HeldItem
        {
            get => helditem;
            set
            {
                gameObject.GetComponent<Inventory>().SetCurItem(value);
                helditem = value;
            }
        }

        /// <summary>
        /// Get / Set the BadgeText of the Dummy
        /// </summary>
        public string BadgeName
        {
            get => gameObject.GetComponent<ServerRoles>().MyText;
            set => gameObject.GetComponent<ServerRoles>().SetText(value);
        }

        /// <summary>
        /// Get / Set the BadgeCOlor of the Dummy
        /// </summary>
        public string BadgeColor
        {
            get => gameObject.GetComponent<ServerRoles>().MyColor;
            set => gameObject.GetComponent<ServerRoles>().SetColor(value);
        }

        /// <summary>
        /// Creates a New Dummy and Spawn it
        /// </summary>
        /// <param name="pos">The Position where the Dummy should spawn</param>
        /// <param name="rot">The Rotation of the Dummy</param>
        /// <param name="role">The Role which the Dummy should be</param>
        /// <param name="name">The Name of the Dummy</param>
        /// <param name="badgetext">The Displayed BadgeTeyt of the Dummy</param>
        /// <param name="badgecolor">The Displayed BadgeColor of the Dummy</param>
        public Dummy(Vector3 pos, Quaternion rot, RoleType role = RoleType.ClassD, string name = "(null)",string badgetext = "",string badgecolor = "")
        {
            GameObject obj =
                Object.Instantiate(
                    NetworkManager.singleton.spawnPrefabs.FirstOrDefault(p => p.gameObject.name == "Player"));

            if (obj.GetComponent<Player>() == null)
                obj.AddComponent<Player>();

            gameObject = obj;

            obj.GetComponent<CharacterClassManager>().CurClass = role;
            obj.GetComponent<NicknameSync>().Network_myNickSync = name;
            gameObject.GetComponent<ServerRoles>().MyText = badgetext;
            gameObject.GetComponent<ServerRoles>().MyColor = badgecolor;
            obj.transform.localScale = Vector3.one;
            obj.transform.position = pos;
            obj.GetComponent<QueryProcessor>().NetworkPlayerId = 9999;
            obj.GetComponent<QueryProcessor>().PlayerId = 9999;

            NetworkServer.Spawn(obj);
            ReferenceHub.Hubs.Remove(obj);
        }

        /// <summary>
        /// Despawns the Dummy
        /// </summary>
        public void Despawn() => NetworkServer.UnSpawn(gameObject);

        /// <summary>
        /// Spawns the Dummy again after Despawning
        /// </summary>
        public void Spawn() => NetworkServer.Spawn(gameObject);

        /// <summary>
        /// Destroys the Object
        /// </summary>
        public void Destroy() => Object.Destroy(gameObject);
    }
}
