using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Synapse.Api
{
    public static class Map
    {
		// Variables
		private static List<Room> _rooms = new List<Room>();
		// Methods
		/// <summary>Gives you the Position of the Door</summary>
		/// <param name="doorName">Name of the Door you want</param>
		/// <returns></returns>
		public static Vector3 GetDoorPos(string doorName)
		{
			var door = Object.FindObjectsOfType<Door>().FirstOrDefault((dr) => dr.DoorName.ToUpper() == doorName);
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
				if (FallDamage.CheckUnsafePosition(vector))
				{
					break;
				}
				if (b == 20)
				{
					vector = Vector3.zero;
				}
			}

			return vector;
		}

		/// <summary>Gives you the Position of the cubes you can see when you write "showrids" in the console!</summary>
		/// <param name="room"></param>
		/// <returns></returns>
		public static Vector3 GetRidPos(string room)
		{
			var position = new Vector3(53f, 1020f, -44f);
			var array = GameObject.FindGameObjectsWithTag("RoomID");
			foreach (var gameObject2 in array)
			{
				if (gameObject2.GetComponent<Rid>().id == room)
				{
					position = gameObject2.transform.position;
				}
			}
			return position;
		}

		/// <param name="name">The name of the Room you want</param>
		/// <returns>Gives you the Position of the Room</returns>
		public static Vector3 GetRoomPos(string name) => Rooms.Where(room => room.Name.ToUpper() == name.ToUpper()).FirstOrDefault().Position;

		/// <summary>Gives you the Spawn Position of a Role</summary>
		/// <param name="type">The Role you want to get s spawn position</param>
		/// <returns></returns>
		public static Vector3 GetRandomSpawnPoint(this RoleType type) => Object.FindObjectOfType<SpawnpointManager>().GetRandomPosition(type).transform.position;
		
		/// <summary>Gives You a List with all Rooms on the Server</summary>
		public static List<Room> Rooms
        {
            get
            {
				if (_rooms == null || _rooms.Count == 0)
					_rooms = Object.FindObjectsOfType<Transform>().Where(transform => transform.CompareTag("Room")).Select(obj => new Room { Name = obj.name, Position = obj.position, Transform = obj.transform }).ToList();

				return _rooms;
            }
        }

		/// <summary>Starts the AlphaWarhead</summary>
		public static void StartNuke()
        {
			var alpha = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();
			alpha.InstantPrepare();
			alpha.StartDetonation();
        }

		/// <summary>Stops the AlphaWarhead</summary>
		public static void StopNuke()
        {
			var alpha = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();
			alpha.CancelDetonation();
		}

		/// <summary>Detonate the AlphaWarhead instantly</summary>
		public static void DetonateNuke()
        {
			var alpha = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();
			alpha.Detonate();
		}

		/// <summary>Gets The Status of is the NukeDetonated</summary>
		public static bool IsNukeDetonated => PlayerManager.localPlayer.GetComponent<AlphaWarheadController>().detonated;

		/// <summary>Gets The Status of is the NukeInProgress</summary>
		public static bool IsNukeInProgress => PlayerManager.localPlayer.GetComponent<AlphaWarheadController>().inProgress;
	}
}
