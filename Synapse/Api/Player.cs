using System;
using System.Linq;
using Hints;
using Mirror;
using Synapse.Permissions;
using UnityEngine;

namespace Synapse.Api
{
    public class Player : MonoBehaviour
    {
        public static Player Server { get => PlayerManager.localPlayer.GetPlayer(); }


        public ReferenceHub Hub { get => this.GetComponent<ReferenceHub>(); }

        public CharacterClassManager ClassManager { get => Hub.characterClassManager; }

        public PlayerEffectsController EffectsController { get => Hub.playerEffectsController; }

        public string NickName { get => Hub.nicknameSync.MyNick; set => Hub.nicknameSync.MyNick = value; }

        public int PlayerId { get => Hub.queryProcessor.NetworkPlayerId; set => Hub.queryProcessor.NetworkPlayerId = value; }

        public string UserID { get => Hub.characterClassManager.UserId; set => Hub.characterClassManager.UserId = value; }

        public bool OverWatch { get => Hub.serverRoles.OverwatchEnabled; set => Hub.serverRoles.OverwatchEnabled = value; }

        public bool Bypass { get => Hub.serverRoles.BypassMode; set => Hub.serverRoles.BypassMode = value; }

        public bool GodMode { get => ClassManager.GodMode; set => ClassManager.GodMode = value; }

        public Vector3 Scale 
        { 
            get => Hub.transform.localScale;
            set
            {
                try
                {
                    Hub.transform.localScale = value;

                    foreach (var player in PlayerExtensions.GetAllPlayers())
                        PlayerExtensions.SendSpawnMessage?.Invoke(null, new object[] { Hub.GetComponent<NetworkIdentity>(), player.Connection });
                }
                catch (Exception e)
                {
                    Log.Error($"SetScale Error. {e}");
                }
            }
        }

        public Vector3 Position { get => Hub.playerMovementSync.transform.position; set => Hub.playerMovementSync.OverridePosition(value,0f,true); }

        public Vector3 RotationVector { get => ClassManager._plyCam.transform.forward; set => ClassManager._plyCam.transform.forward = value; }

        public Vector2 Rotation { get => Hub.playerMovementSync.RotationSync; set => Hub.playerMovementSync.RotationSync = value; }

        public float Health { get => Hub.playerStats.Health; set => Hub.playerStats.Health = value; }

        public int MaxHealth { get => Hub.playerStats.maxHP; set => Hub.playerStats.maxHP = value; }

        public float ArtificialHealth { get => Hub.playerStats.unsyncedArtificialHealth; set => Hub.playerStats.unsyncedArtificialHealth = value; }

        public int MaxArtificialHealth { get => Hub.playerStats.maxArtificialHealth; set => Hub.playerStats.maxArtificialHealth = value; }

        public RoleType Role
        {
            get => Hub.characterClassManager.NetworkCurClass;
            set => Hub.characterClassManager.SetPlayersClass(value, gameObject);
        }

        public Team Team { get => Hub.characterClassManager.CurRole.team; set => Hub.characterClassManager.CurRole.team = value; }

        public Room CurRoom
        {
            get
            {
                var playerPos = Position;
                var end = playerPos - new Vector3(0f, 10f, 0f);
                var flag = Physics.Linecast(playerPos, end, out var rayCastHit, -84058629);

                if (!flag || rayCastHit.transform == null)
                    return null;

                var transform = rayCastHit.transform;

                while (transform.parent != null && transform.parent.parent != null)
                    transform = transform.parent;

                foreach (var room in Map.Rooms.Where(room => room.Position == transform.position))
                    return room;

                return new Room
                {
                    Name = transform.name,
                    Position = transform.position,
                    Transform = transform
                };
            }
            set => Position = Map.Rooms.FirstOrDefault(x => x == value).Position;
        }

        public NetworkConnection Connection { get => Hub.scp079PlayerScript.connectionToClient; }

        public Inventory.SyncListItemInfo Items { get => Hub.inventory.items; set => Hub.inventory.items = value; }

        public Player Cuffer { get => PlayerExtensions.GetPlayer(GetComponent<Handcuffs>().CufferId); } 

        public uint Ammo5 { get => Hub.ammoBox.amount[0]; set => Hub.ammoBox.amount[0] = value; }

        public uint Ammo7 { get => Hub.ammoBox.amount[1]; set => Hub.ammoBox.amount[1] = value; }

        public uint Ammo9 { get => Hub.ammoBox.amount[2]; set => Hub.ammoBox.amount[2] = value; }

        public UserGroup Rank 
        {
            get => Hub.serverRoles.Group; 
            set
            {
                Hub.serverRoles.SetGroup(value, false, false, false);
            }
        }

        public string RankColor { get => Rank.BadgeColor; set => Hub.serverRoles.SetColor(value); }

        public string RankName { get => Rank.BadgeText; set => Hub.serverRoles.SetText(value); }

        public bool IsMuted { get => ClassManager.NetworkMuted; set => ClassManager.NetworkMuted = value; }

        public bool IsIntercomMuted { get => ClassManager.NetworkIntercomMuted; set => ClassManager.NetworkIntercomMuted = value; }


        public void Kick(string message) => ServerConsole.Disconnect(gameObject, message);

        public void ChangeRoleAtPosition(RoleType role)
        {
            Hub.characterClassManager.NetworkCurClass = role;
            Hub.playerStats.SetHPAmount(Hub.characterClassManager.Classes.SafeGet(Role).maxHP);
        }

        public bool CheckPermission(string permission)
        {
            if (Hub == PlayerManager.localPlayer) return true;
            try
            {
                return PermissionReader.CheckPermission(this, permission);
            }
            catch
            {
                return false;
            }
        }

        public void GiveTextHint(string message, float duration = 5f)
        {
            Hub.hints.Show(new TextHint(message, new HintParameter[]
                {
                    new StringHintParameter("")
                }, HintEffectPresets.FadeInAndOut(duration), duration));
        }

        public void ClearBroadcasts() => GetComponent<Broadcast>().TargetClearElements(Connection);

        public void Broadcast(ushort time,string message) => GetComponent<Broadcast>().TargetAddElement(Connection, message, time, new Broadcast.BroadcastFlags());

        public void InstantBroadcast(ushort time, string message)
        {
            ClearBroadcasts();
            GetComponent<Broadcast>().TargetAddElement(Connection, message, time, new Broadcast.BroadcastFlags());
        }

        public void SendConsoleMessage(string message, string color) => ClassManager.TargetConsolePrint(Connection, message, color);

        public void HideTag() => ClassManager.CallCmdRequestHideTag();

        public void ShowTag(bool global = false) => ClassManager.CallCmdRequestShowTag(global);

        public void GiveItem(ItemType itemType, float duration = float.NegativeInfinity, int sight = 0, int barrel = 0, int other = 0) => Hub.inventory.AddNewItem(itemType, duration, sight, barrel, other);
    }
}
