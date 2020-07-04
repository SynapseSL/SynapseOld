using System.Collections.Generic;
using UnityEngine;

namespace Synapse.Api
{
    public class Jail : MonoBehaviour
    {
        public bool IsJailed { get; private set; }
        
        public Player Player => this.GetPlayer();

        public Player Admin { get; set; }
        
        public RoleType Role { get; set; }
        
        public Vector3 Position { get; set; }
        
        public List<Inventory.SyncItemInfo> Items { get; set; }
        
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
            
            //TODO: Fix Player Position

            Items = new List<Inventory.SyncItemInfo>();
            foreach (var item in player.Items)
                Items.Add(item);

            Health = player.Health;

            player.Role = RoleType.Tutorial;

            IsJailed = true;
        }

        public void UnJail()
        {
            if (!IsJailed) return;

            var player = this.GetPlayer();
            player.Role = Role;
            player.Health = Health;
            
            //TODO: Fix Player Position

            foreach (var item in Items)
                player.Inventory.items.Add(item);

            IsJailed = false;
        }
    }
}