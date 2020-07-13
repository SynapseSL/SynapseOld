using LightContainmentZoneDecontamination;
using Synapse.Permissions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synapse.Api
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Map
    {
        /// <summary>
        /// Gives you the WarheadController.
        /// </summary>
        public static AlphaWarheadController WarheadController => Player.Server.GetComponent<AlphaWarheadController>();

        /// <summary>
        /// Activates/Deactivates the RoundLock (if the Round can end)
        /// </summary>
        public static bool RoundLock { get => RoundSummary.RoundLock; set => RoundSummary.RoundLock = value; }

        /// <summary>
        /// Activates/Deactivates the LobbyLock (if the Lobby can continue counting down)
        /// </summary>
        public static bool LobbyLock { get => GameCore.RoundStart.LobbyLock; set => GameCore.RoundStart.LobbyLock = value; }

        /// <summary>
        /// Activates/Deactivates the FriendlyFire on the server
        /// </summary>
        public static bool FriendlyFire { get => ServerConsole.FriendlyFire; set => ServerConsole.FriendlyFire = value; }

        /// <summary>
        /// Gives you a list of all lifts
        /// </summary>
        public static List<Lift> Lifts => Object.FindObjectsOfType<Lift>().ToList();

        private static Broadcast BroadcastComponent => Player.Server.GetComponent<Broadcast>();

        private static List<Room> _rooms = new List<Room>();

        /// <summary>
        /// Gives you a list of all rooms
        /// </summary>
        public static List<Room> Rooms
        {
            get
            {
                if (_rooms == null || _rooms.Count == 0)
                    _rooms = Object.FindObjectsOfType<Transform>().Where(transform => transform.CompareTag("Room"))
                        .Select(obj => new Room {Name = obj.name, Position = obj.position, Transform = obj.transform})
                        .ToList();

                return _rooms;
            }
        }

        /// <summary>
        /// Is the nuke detonated?
        /// </summary>
        public static bool IsNukeDetonated =>
            WarheadController.detonated;

        /// <summary>
        /// Is the nuke in progress?
        /// </summary>
        public static bool IsNukeInProgress =>
            WarheadController.inProgress;

        /// <summary>
        /// How many generators are activated?
        /// </summary>
        public static int ActivatedGenerators => Generator079.mainGenerator.totalVoltage;

        // Methods
        /// <summary>Gives the position of the door with that name</summary>
        /// <param name="doorName">Name of the Door you want</param>
        /// <returns></returns>
        public static Vector3 GetDoorPos(string doorName)
        {
            var door = Object.FindObjectsOfType<Door>().FirstOrDefault(dr => dr.DoorName.ToUpper() == doorName);
            if (door == null) return Vector3.one;
            var vector = door.transform.position;
            vector.y += 2.5f;
            for (byte b = 0; b < 21; b += 1)
            {
                if (b == 0)
                {
                    vector.x += 1.5f;
                }
                else if (b < 3)
                {
                    vector.x += 1f;
                }
                else if (b == 4)
                {
                    vector = door.transform.position;
                    vector.y += 2.5f;
                    vector.z += 1.5f;
                }
                else if (b < 10 && b % 2 == 0)
                {
                    vector.z += 1f;
                }
                else if (b < 10)
                {
                    vector.x += 1f;
                }
                else if (b == 10)
                {
                    vector = door.transform.position;
                    vector.y += 2.5f;
                    vector.x -= 1.5f;
                }
                else if (b < 13)
                {
                    vector.x -= 1f;
                }
                else if (b == 14)
                {
                    vector = door.transform.position;
                    vector.y += 2.5f;
                    vector.z -= 1.5f;
                }
                else if (b % 2 == 0)
                {
                    vector.z -= 1f;
                }
                else
                {
                    vector.x -= 1f;
                }

                if (FallDamage.CheckUnsafePosition(vector)) break;
                if (b == 20) vector = Vector3.zero;
            }

            return vector;
        }

        /// <summary>
        /// Gives you the position of the cubes you can see when you write "showrids" in the console
        /// </summary>
        /// <param name="ridname"></param>
        /// <returns></returns>
        public static Vector3 GetRidPos(string ridname)
        {
            var position = new Vector3(53f, 1020f, -44f);
            var array = GameObject.FindGameObjectsWithTag("RoomID");
            foreach (var gameObject2 in array)
                if (gameObject2.GetComponent<Rid>().id == ridname)
                    position = gameObject2.transform.position;
            return position;
        }

        /// <summary>
        /// Gives the position of the room with that name
        /// </summary>
        /// <param name="name">The name of the Room you want</param>
        public static Vector3 GetRoomPos(string name)
        {
            return Rooms?.FirstOrDefault(room =>
                       string.Equals(room.Name, name, StringComparison.CurrentCultureIgnoreCase))?.Position ??
                   new Vector3(0f, 0f, 0f);
        }

        /// <summary>
        /// Gives you a random spawnpoint of the Role
        /// </summary>
        /// <param name="type">The Role you want to get s spawn position</param>
        /// <returns></returns>
        public static Vector3 GetRandomSpawnPoint(this RoleType type)
        {
            return Object.FindObjectOfType<SpawnpointManager>().GetRandomPosition(type).transform.position;
        }

        /// <summary>
        /// Sends a Cassie Message
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="makeHold"></param>
        /// <param name="makeNoise"></param>
        public static void CassieMessage(string msg, bool makeHold, bool makeNoise) => Respawning.RespawnEffectsController.PlayCassieAnnouncement(msg, makeHold, makeNoise);

        /// <summary>
        ///  Starts the Decontamination
        /// </summary>
        public static void StartDecontamination() => DecontaminationController.Singleton.FinishDecontamination();


        /// <summary>
        /// Starts the nuke
        /// </summary>
        public static void StartNuke()
        {
            var alpha = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();
            alpha.InstantPrepare();
            alpha.StartDetonation();
        }

        /// <summary>
        /// Spawns a Item on the Map
        /// </summary>
        public static Pickup SpawnItem(ItemType itemType, float durability, Vector3 position, Quaternion rotation = default, int sight = 0, int barrel = 0, int other = 0)
            => Player.Server.Inventory.SetPickup(itemType, durability, position, rotation, sight, barrel, other);

        /// <summary>
        /// Stops the nuke
        /// </summary>
        public static void StopNuke() => WarheadController.CancelDetonation();

        /// <summary>
        /// Detonates the nuke
        /// </summary>
        public static void DetonateNuke() => WarheadController.Detonate();

        /// <summary>
        /// Has the group the permission?
        /// </summary>
        /// <param name="group">Name of the group you want to check</param>
        /// <param name="permission">Permission you want to check</param>
        public static bool IsGroupAllowed(string group, string permission)
        {
            try
            {
                return PermissionReader.CheckGroupPermission(group, permission);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gives all players a broadcast
        /// </summary>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public static void Broadcast(string message, ushort duration) => BroadcastComponent.RpcAddElement(message, duration, new Broadcast.BroadcastFlags());

        /// <summary>
        /// Clear all broadcasts from all players
        /// </summary>
        public static void ClearBroadcasts() => BroadcastComponent.RpcClearElements();

        /// <summary>
        /// Deactivates the lights
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="onlyHeavy"></param>
        public static void TurnOffAllLights(float duration, bool onlyHeavy = false) => Generator079.Generators[0].RpcCustomOverchargeForOurBeautifulModCreators(duration, onlyHeavy);
    }
}