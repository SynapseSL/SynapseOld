using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Synapse.Api
{
    public class Jail : MonoBehaviour
    {
        /// <summary>
        /// Is the Player currently in Jail?
        /// </summary>
        public bool IsJailed { get; private set; }
        
        /// <summary>
        /// The Player which is Jailed
        /// </summary>
        public Player Player => this.GetPlayer();

        /// <summary>
        /// The Admins which Jailed the Player
        /// </summary>
        public Player Admin { get; set; }
        
        /// <summary>
        /// The Role the Player will get after he gets Unjailed
        /// </summary>
        public RoleType Role { get; set; }


        /// <summary>
        /// The Position the Player will get after he gets Unjailed
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The Items the Player will get after he gets Unjailed
        /// </summary>
        public List<Inventory.SyncItemInfo> Items { get; set; }

        /// <summary>
        /// The Health the Player will get after he gets Unjailed
        /// </summary>
        public float Health { get; set; }

        /// <summary>
        /// Used by Unity for you its Usseles
        /// </summary>
        public void Awake()
        {
            IsJailed = false;
            Role = RoleType.ClassD;
            Position = Role.GetRandomSpawnPoint();
        }

        /// <summary>
        /// Jail the Player
        /// </summary>
        /// <param name="admin">The Person who jails the Player</param>
        public void DoJail(Player admin)
        {
            if (IsJailed) return;

            var player = this.GetPlayer();

            Admin = admin;
            Role = player.Role;
            Position = player.Position;

            Items = new List<Inventory.SyncItemInfo>();
            foreach (var item in player.Items)
                Items.Add(item);

            Health = player.Health;

            player.Role = RoleType.Tutorial;

            IsJailed = true;
        }

        /// <summary>
        /// Unjail the Player
        /// </summary>
        public void UnJail()
        {
            if (!IsJailed) return;

            var player = this.GetPlayer();
            player.Role = Role;
            Timing.CallDelayed(0.2f, () => player.Position = Position);
            player.Health = Health;

            foreach (var item in Items)
                player.Inventory.items.Add(item);

            IsJailed = false;
        }
    }
}