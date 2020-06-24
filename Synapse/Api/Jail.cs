using System;
using UnityEngine;

namespace Synapse.Api
{
    public class Jail : MonoBehaviour
    {
        public bool IsJailed { get; private set; }
        
        public Player Player
        {
            get => this.GetPlayer();
        }
        
        public Player Admin { get; set; }
        
        public RoleType Role { get; set; }
        
        public Vector3 Position { get; set; }
        
        public Inventory.SyncListItemInfo Items { get; set; }
        
        public float Health { get; set; }


        public void Awake()
        {
            IsJailed = false;
            Role = RoleType.ClassD;
            Position = Role.GetRandomSpawnPoint();
        }

        public void DoJail(Player admin)
        {
            if (IsJailed) return;

            var player = this.GetPlayer();

            Admin = admin;
            Role = player.Role;
            Position = player.Position;
            if (Items != null) Items = player.Items;
            Health = player.Health;

            player.Role = RoleType.Tutorial;

            IsJailed = true;
        }

        public void UnJail()
        {
            if (!IsJailed) return;

            var player = this.GetPlayer();
            player.Role = Role;
            player.Position = Position;
            player.Health = Health;
            player.Items = Items;

            IsJailed = false;
        }
    }
}