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
        public static AlphaWarheadController WarheadController => Player.Server.GetComponent<AlphaWarheadController>();

        public static bool RoundLock { get => RoundSummary.RoundLock; set => RoundSummary.RoundLock = value; }

        public static bool LobbyLock { get => GameCore.RoundStart.LobbyLock; set => GameCore.RoundStart.LobbyLock = value; }

        public static bool FriendlyFire
        {
            get => ServerConsole.FriendlyFire;
            set
            {
                ServerConsole.FriendlyFire = value;
                foreach (var player in PlayerExtensions.GetAllPlayers())
                    player.FriendlyFire = value;
            }
        }

        public static List<Lift> Lifts => Object.FindObjectsOfType<Lift>().ToList();

        private static Broadcast BroadcastComponent => Player.Server.GetComponent<Broadcast>();

        // Variables
        private static List<Room> _rooms = new List<Room>();

        /// <summary>Gives You a List with all Rooms on the Server</summary>
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

        /// <summary>Gets The Status of is the NukeDetonated</summary>
        public static bool IsNukeDetonated =>
            WarheadController.detonated;

        /// <summary>Gets The Status of is the NukeInProgress</summary>
        public static bool IsNukeInProgress =>
            WarheadController.inProgress;

        public static int ActivatedGenerators => Generator079.mainGenerator.totalVoltage;

        // Methods
        /// <summary>Gives you the Position of the Door</summary>
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

        /// <summary>Gives you the Position of the cubes you can see when you write "showrids" in the console!</summary>
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

        /// <param name="name">The name of the Room you want</param>
        /// <returns>Gives you the Position of the Room</returns>
        public static Vector3 GetRoomPos(string name)
        {
            return Rooms?.FirstOrDefault(room =>
                       string.Equals(room.Name, name, StringComparison.CurrentCultureIgnoreCase))?.Position ??
                   new Vector3(0f, 0f, 0f);
        }

        /// <summary>Gives you the Spawn Position of a Role</summary>
        /// <param name="type">The Role you want to get s spawn position</param>
        /// <returns></returns>
        public static Vector3 GetRandomSpawnPoint(this RoleType type)
        {
            return Object.FindObjectOfType<SpawnpointManager>().GetRandomPosition(type).transform.position;
        }

        /// <summary>Starts the AlphaWarhead</summary>
        public static void StartNuke()
        {
            var alpha = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();
            alpha.InstantPrepare();
            alpha.StartDetonation();
        }

        /// <summary>Stops the AlphaWarhead</summary>
        public static void StopNuke() => WarheadController.CancelDetonation();

        /// <summary>Detonate the AlphaWarhead instantly</summary>
        public static void DetonateNuke() => WarheadController.Detonate();

        /// <param name="group">Name of the group you want to check</param>
        /// <param name="permission">Permission you want to check</param>
        /// <returns>Have the Group the permissions?</returns>
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

        public static void Broadcast(string message, ushort duration) => BroadcastComponent.RpcAddElement(message, duration, new Broadcast.BroadcastFlags());

        public static void ClearBroadcasts() => BroadcastComponent.RpcClearElements();

        public static void TurnOffAllLights(float duration, bool onlyHeavy = false) => Generator079.generators[0].RpcCustomOverchargeForOurBeautifulModCreators(duration, onlyHeavy);
    }
}