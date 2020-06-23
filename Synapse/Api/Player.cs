using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hints;
using Mirror;
using Synapse.Api.Enums;
using Synapse.Permissions;
using UnityEngine;

namespace Synapse.Api
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Player : MonoBehaviour
    {
        public static Player Server => PlayerManager.localPlayer.GetPlayer();


        public ReferenceHub Hub => GetComponent<ReferenceHub>();

        public CharacterClassManager ClassManager => Hub.characterClassManager;

        public PlayerEffectsController EffectsController => Hub.playerEffectsController;

        public string NickName { get => Hub.nicknameSync.Network_myNickSync; set => Hub.nicknameSync.Network_myNickSync = value; }

        public int PlayerId { get => Hub.queryProcessor.NetworkPlayerId; set => Hub.queryProcessor.NetworkPlayerId = value; }

        public string UserId { get => Hub.characterClassManager.UserId; set => Hub.characterClassManager.UserId = value; }

        public string CustomUserId { get => ClassManager.UserId2; set => ClassManager.UserId2 = value; }

        public string IpAddress => Hub.queryProcessor._ipAddress;

        public bool NoClip { get => Hub.serverRoles.NoclipReady; set => Hub.serverRoles.NoclipReady = value; }

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
                    Log.Error($"SetScale Error: {e}");
                }
            }
        }

        public Vector3 Position { get => Hub.playerMovementSync.GetRealPosition(); set => Hub.playerMovementSync.OverridePosition(value,RotationFloat); }

        public Vector3 RotationVector { get => ClassManager._plyCam.transform.forward; set => ClassManager._plyCam.transform.forward = value; }

        public Vector2 Rotation { get => Hub.playerMovementSync.RotationSync; set => Hub.playerMovementSync.RotationSync = value; }

        public float Health { get => Hub.playerStats.Health; set => Hub.playerStats.Health = value; }

        public int MaxHealth { get => Hub.playerStats.maxHP; set => Hub.playerStats.maxHP = value; }

        public float ArtificialHealth { get => Hub.playerStats.unsyncedArtificialHealth; set => Hub.playerStats.unsyncedArtificialHealth = value; }

        public int MaxArtificialHealth { get => Hub.playerStats.maxArtificialHealth; set => Hub.playerStats.maxArtificialHealth = value; }

        public RoleType Role
        {
            get => Hub.characterClassManager.CurClass;
            set => Hub.characterClassManager.SetPlayersClass(value,gameObject);
        }

        public Team Team { get => Hub.characterClassManager.CurRole.team; set => Hub.characterClassManager.CurRole.team = value; }

        public Team Side
        {
            get
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (Team)
                {
                    case Team.RSC:
                        return Team.MTF;

                    case Team.CDP:
                        return Team.CHI;

                    default:
                        return Team;
                }
            }
        }

        public Room CurRoom
        {
            get
            {
                var playerPos = Position;
                var end = playerPos - new Vector3(0f, 10f, 0f);
                var flag = Physics.Linecast(playerPos, end, out var rayCastHit, -84058629);

                if (!flag || rayCastHit.transform == null)
                    return null;

                var infoTransform = rayCastHit.transform;

                while (infoTransform.parent != null && infoTransform.parent.parent != null)
                    infoTransform = infoTransform.parent;

                foreach (var room in Map.Rooms.Where(room => room.Position == infoTransform.position))
                    return room;

                return new Room
                {
                    Name = infoTransform.name,
                    Position = infoTransform.position,
                    Transform = infoTransform
                };
            }
            set => Position = value.Position;
        }

        public NetworkConnection Connection => Hub.scp079PlayerScript.connectionToClient;

        public Inventory.SyncListItemInfo Items { get => Hub.inventory.items; set => Hub.inventory.items = value; }

        /// <summary>
        /// The Person Who has cuffed the Player
        /// </summary>
        /// <remarks>Set Cuffer to null and he will be disarmed</remarks>
        public Player Cuffer 
        { 
            get => PlayerExtensions.GetPlayer(Hub.handcuffs.CufferId);
            set
            {
                var handcuff = value.Hub.handcuffs;

                if (handcuff == null) return;

                if (value == null)
                {
                    handcuff.NetworkCufferId = -1;
                    return;
                }

                handcuff.NetworkCufferId = value.PlayerId;
            }
        } 

        public uint Ammo5 { get => Hub.ammoBox.amount[0]; set => Hub.ammoBox.amount[0] = value; }

        public uint Ammo7 { get => Hub.ammoBox.amount[1]; set => Hub.ammoBox.amount[1] = value; }

        public uint Ammo9 { get => Hub.ammoBox.amount[2]; set => Hub.ammoBox.amount[2] = value; }

        public UserGroup Rank { get => Hub.serverRoles.Group; set => Hub.serverRoles.SetGroup(value, false); }

        public string GroupName => ServerStatic.PermissionsHandler._members[UserId];

        public string RankColor { get => Rank.BadgeColor; set => Hub.serverRoles.SetColor(value); }

        public string RankName { get => Rank.BadgeText; set => Hub.serverRoles.SetText(value); }

        public bool IsMuted { get => ClassManager.NetworkMuted; set => ClassManager.NetworkMuted = value; }

        public bool IsIntercomMuted { get => ClassManager.NetworkIntercomMuted; set => ClassManager.NetworkIntercomMuted = value; }

        public bool FriendlyFire { get => Hub.weaponManager.NetworkfriendlyFire; set => Hub.weaponManager.NetworkfriendlyFire = value; }

        public Camera079 Camera { get => Hub.scp079PlayerScript.currentCamera; set => Hub.scp079PlayerScript?.RpcSwitchCamera(value.cameraId, false); }


        public float RotationFloat => Hub.transform.rotation.eulerAngles.y;

        public bool IsCuffed => Cuffer != null;

        public bool IsReloading => Hub.weaponManager.IsReloading();

        public bool IsZooming => Hub.weaponManager.ZoomInProgress();

        public bool IsDead => Team == Team.RIP;

        public Inventory Inventory => Hub.inventory;


        //Methods
        public void Kick(string message) => ServerConsole.Disconnect(gameObject, message);

        public void Ban(int duration, string reason, string issuer = "Plugin") => Server.GetComponent<BanPlayer>().BanUser(gameObject, duration, reason, issuer);

        public void Kill(DamageTypes.DamageType damageType = default) => Hub.playerStats.HurtPlayer(new PlayerStats.HitInfo(-1f, "WORLD", damageType, 0), gameObject);

        public void ChangeRoleAtPosition(RoleType role)
        {
            //TODO: Fix this shit
            Hub.characterClassManager.SetClassIDAdv(role, true);
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

        public void ClearInventory() => Hub.inventory.Clear();

        public void GiveEffect(Effect effect,byte intensity = 1,float duration = -1f) => EffectsController.ChangeByString(effect.ToString().ToLower(), intensity, duration);

        public void RaLogin()
        {
            Hub.serverRoles.RemoteAdmin = true;
            Hub.serverRoles.Permissions = Hub.serverRoles.Group.Permissions;
            Hub.serverRoles.RemoteAdminMode = ServerRoles.AccessMode.PasswordOverride;
            Hub.serverRoles.TargetOpenRemoteAdmin(Connection, false);
        }

        public void RaLogout()
        {
            Hub.serverRoles.RemoteAdmin = false;
            Hub.serverRoles.Permissions = 0UL;
            Hub.serverRoles.RemoteAdminMode = ServerRoles.AccessMode.LocalAccess;
            Hub.serverRoles.TargetCloseRemoteAdmin(Connection);
        }
    }
}
