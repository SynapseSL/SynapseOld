using System.Linq;
using UnityEngine;
using Mirror;

namespace Synapse.Api
{
    public static class Map
    {
		/// <summary>Gives you the Position of the Door</summary>
		/// <param name="Doorname">Name of the Door you want</param>
		/// <returns></returns>
		public static Vector3 GetDoorpos(string Doorname)
		{
			Vector3 vector = Vector3.down;
			Door door = UnityEngine.Object.FindObjectsOfType<Door>().FirstOrDefault((Door dr) => dr.DoorName.ToUpper() == Doorname);
			vector = door.transform.position;
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
		/// <param name="Room"></param>
		/// <returns></returns>
		public static Vector3 GetRidPos(string Room)
		{
			Vector3 position = new Vector3(53f, 1020f, -44f);
			GameObject[] array = GameObject.FindGameObjectsWithTag("RoomID");
			foreach (GameObject gameObject2 in array)
			{
				if (gameObject2.GetComponent<Rid>().id == Room)
				{
					position = gameObject2.transform.position;
				}
			}
			return position;
		}

		/// <summary>Gives you the Spawn Position of a Role</summary>
		/// <param name="type">The Role you want to get s spawn position</param>
		/// <returns></returns>
		public static Vector3 GetRandomSpawnpoint(this RoleType type) => UnityEngine.Object.FindObjectOfType<SpawnpointManager>().GetRandomPosition(type).transform.position;
	}
}
