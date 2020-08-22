using Mirror;
using RemoteAdmin;
using System.Linq;
using UnityEngine;

namespace Synapse.Api
{
    public class Dummy
    {
        private ItemType helditem;
        private GameObject gameObject;

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

        public string Name
        {
            get => gameObject.GetComponent<NicknameSync>().Network_myNickSync;
            set => gameObject.GetComponent<NicknameSync>().Network_myNickSync = value;
        }

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

        public ItemType HeldItem
        {
            get => helditem;
            set
            {
                gameObject.GetComponent<Inventory>().SetCurItem(value);
                helditem = value;
            }
        }

        public string BadgeName
        {
            get => gameObject.GetComponent<ServerRoles>().MyText;
            set => gameObject.GetComponent<ServerRoles>().SetText(value);
        }

        public string BadgeColor
        {
            get => gameObject.GetComponent<ServerRoles>().MyColor;
            set => gameObject.GetComponent<ServerRoles>().SetColor(value);
        }


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

        public void Despawn() => NetworkServer.UnSpawn(gameObject);

        public void Spawn() => NetworkServer.Spawn(gameObject);

        public void Destroy() => Object.Destroy(gameObject);
    }
}
