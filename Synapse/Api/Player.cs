﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hints;
using Mirror;
using RemoteAdmin;
using Searching;
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

        public Inventory Inventory => Hub.inventory;

        public PlayerStats PlayerStats => Hub.playerStats;

        public ServerRoles ServerRoles => Hub.serverRoles;

        public QueryProcessor QueryProcessor => Hub.queryProcessor;

        public PlayerMovementSync MovementSync => Hub.playerMovementSync;

        public NicknameSync NicknameSync => Hub.nicknameSync;

        public SpectatorManager SpectatorManager => Hub.spectatorManager;

        public AnimationController AnimationController => Hub.animationController;

        public FallDamage FallDamage => Hub.falldamage;

        public Handcuffs Handcuffs => Hub.handcuffs;

        public PlayerInteract PlayerInteract => Hub.playerInteract;

        public PlayerEffectsController EffectsController => Hub.playerEffectsController;

        public FootstepSync FootstepSync => Hub.footstepSync;

        public SearchCoordinator SearchCoordinator => Hub.searchCoordinator;

        public HintDisplay HintDisplay => Hub.hints;

        /// <summary>
        /// The CommandSender objects of the Player
        /// </summary>
        public CommandSender CommandSender
        {
            get
            {
                if (this == Server) return ServerConsole._scs;
                else return QueryProcessor._sender;
            }
        }

        /// <summary>
        /// The name of the player
        /// </summary>
        public string NickName { get => NicknameSync.Network_myNickSync; set => Hub.nicknameSync.Network_myNickSync = value; }

        /// <summary>
        /// The PlayerId of the player (The Id you can see in RemoteAdmin)
        /// </summary>
        public int PlayerId { get => QueryProcessor.NetworkPlayerId; set => Hub.queryProcessor.NetworkPlayerId = value; }

        /// <summary>
        /// The UserId of the player (@steam, @discord, etc.)
        /// </summary>
        public string UserId { get => Hub.characterClassManager.UserId; set => Hub.characterClassManager.UserId = value; }

        /// <summary>
        /// A possible second id.
        /// </summary>
        public string CustomUserId { get => ClassManager.UserId2; set => ClassManager.UserId2 = value; }

        /// <summary>
        /// The Ip address of the player
        /// </summary>
        public string IpAddress => QueryProcessor._ipAddress;

        /// <summary>
        /// Get / Set the user into noclip
        /// </summary>
        public bool NoClip { get => ServerRoles.NoclipReady; set => Hub.serverRoles.NoclipReady = value; }

        /// <summary>
        /// Get / Set the Overwatch Status
        /// </summary>
        public bool OverWatch { get => ServerRoles.OverwatchEnabled; set => Hub.serverRoles.OverwatchEnabled = value; }


        /// <summary>
        /// Get / Set the Players Bypassmode.
        /// </summary>
        public bool Bypass { get => ServerRoles.BypassMode; set => Hub.serverRoles.BypassMode = value; }


        /// <summary>
        /// Get / Set the Players GodMode
        /// </summary>
        public bool GodMode { get => ClassManager.GodMode; set => ClassManager.GodMode = value; }


        /// <summary>
        /// Modify the size of the Player
        /// </summary>
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

        /// <summary>
        /// The position of the player
        /// </summary>
        public Vector3 Position { get => MovementSync.GetRealPosition(); set => Hub.playerMovementSync.OverridePosition(value,RotationFloat); }

        /// <summary>
        /// The rotation vector of the player
        /// </summary>
        public Vector3 RotationVector { get => ClassManager._plyCam.transform.forward; set => ClassManager._plyCam.transform.forward = value; }

        /// <summary>
        /// The rotation of the player
        /// </summary>
        public Vector2 Rotation { get => MovementSync.RotationSync; set => Hub.playerMovementSync.RotationSync = value; }

        /// <summary>
        /// The health of the player
        /// </summary>
        public float Health { get => PlayerStats.Health; set => Hub.playerStats.Health = value; }

        /// <summary>
        /// The maximum health a player can get
        /// </summary>
        public int MaxHealth { get => PlayerStats.maxHP; set => Hub.playerStats.maxHP = value; }

        /// <summary>
        /// The extra health of the Player (AP)
        /// </summary>
        public float ArtificialHealth { get => PlayerStats.unsyncedArtificialHealth; set => Hub.playerStats.unsyncedArtificialHealth = value; }

        /// <summary>
        /// The maximum ArtificialHealth a player can get
        /// </summary>
        public int MaxArtificialHealth { get => PlayerStats.maxArtificialHealth; set => Hub.playerStats.maxArtificialHealth = value; }

        /// <summary>
        /// The RoleType of the player
        /// </summary>
        public RoleType Role
        {
            get => Hub.characterClassManager.CurClass;
            set => Hub.characterClassManager.SetPlayersClass(value,gameObject);
        }

        /// <summary>
        /// The team of the player
        /// </summary>
        public Team Team { get => ClassManager.CurRole.team; set => Hub.characterClassManager.CurRole.team = value; }

        /// <summary>
        /// The "Side" the player is on (ClassDs are on TeamChaos, Scientists are on Team MTF)
        /// </summary>
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

        /// <summary>
        /// The Room where the player currently is
        /// </summary>
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

        /// <summary>
        /// The NetworkConnection of the player (often used by vanilla methods)
        /// </summary>
        public NetworkConnection Connection => Hub.scp079PlayerScript.connectionToClient;

        /// <summary>
        /// All items the player has
        /// </summary>
        public Inventory.SyncListItemInfo Items { get => Inventory.items; set => Inventory.items = value; }

        /// <summary>
        /// The person who cuffed the player
        /// </summary>
        /// <remarks>maybe be null, if set to null, uncuffed</remarks>
        public Player Cuffer 
        { 
            get => PlayerExtensions.GetPlayer(Handcuffs.CufferId);
            set
            {
                var handcuff = value.Handcuffs;

                if (handcuff == null) return;

                if (value == null)
                {
                    Handcuffs.NetworkCufferId = -1;
                    return;
                }

                Handcuffs.NetworkCufferId = value.PlayerId;
            }
        }

        /// <summary>
        /// How much Ammo5 has the player
        /// </summary>
        public uint Ammo5 { get => Hub.ammoBox.amount[0]; set => Hub.ammoBox.amount[0] = value; }

        /// <summary>
        /// How much Ammo7 has the player
        /// </summary>
        public uint Ammo7 { get => Hub.ammoBox.amount[1]; set => Hub.ammoBox.amount[1] = value; }

        /// <summary>
        /// How much Ammo9 has the player
        /// </summary>
        public uint Ammo9 { get => Hub.ammoBox.amount[2]; set => Hub.ammoBox.amount[2] = value; }

        /// <summary>
        /// The UserGroup the player is in
        /// </summary>
        public UserGroup Rank { get => ServerRoles.Group; set => ServerRoles.SetGroup(value, value != null ? value.Permissions > 0UL : false,true); }

        /// <summary>
        /// The name of the group the user has
        /// </summary>
        public string GroupName => ServerStatic.PermissionsHandler._members[UserId];

        /// <summary>
        /// The color of the rank
        /// </summary>
        /// <remarks>Note: This will not change the permissions</remarks>
        public string RankColor { get => Rank.BadgeColor; set => Hub.serverRoles.SetColor(value); }

        /// <summary>
        /// The name which is shown
        /// </summary>
        /// <remarks>Note: This will not change the permissions</remarks>
        public string RankName { get => Rank.BadgeText; set => Hub.serverRoles.SetText(value); }

        /// <summary>
        /// The Permission of the Player
        /// </summary>
        public ulong Permission { get => ServerRoles.Permissions; set => ServerRoles.Permissions = value; }

        /// <summary>
        /// Is the player muted
        /// </summary>
        public bool IsMuted { get => ClassManager.NetworkMuted; set => ClassManager.NetworkMuted = value; }

        /// <summary>
        ///  Is the player muted in the intercom
        /// </summary>
        public bool IsIntercomMuted { get => ClassManager.NetworkIntercomMuted; set => ClassManager.NetworkIntercomMuted = value; }

        //TODO: Find a way to make this possible again: public bool FriendlyFire { get => Hub.weaponManager.NetworkfriendlyFire; set => Hub.weaponManager.NetworkfriendlyFire = value; }

        /// <summary>
        /// The current camera the player uses (Scp079 only, if not null)
        /// </summary>
        public Camera079 Camera { get => Hub.scp079PlayerScript.currentCamera; set => Hub.scp079PlayerScript?.RpcSwitchCamera(value.cameraId, false); }

        /// <summary>
        /// The rotation float of the player
        /// </summary>
        public float RotationFloat => Hub.transform.rotation.eulerAngles.y;

        /// <summary>
        /// Is the player cuffed?
        /// </summary>
        public bool IsCuffed => Cuffer != null;

        /// <summary>
        /// Is the player reloading right now?
        /// </summary>
        public bool IsReloading => Hub.weaponManager.IsReloading();

        /// <summary>
        /// Is the player currently scoping
        /// </summary>
        public bool IsZooming => Hub.weaponManager.ZoomInProgress();

        /// <summary>
        ///  Is the player dead
        /// </summary>
        public bool IsDead => Team == Team.RIP;

        public Jail Jail => GetComponent<Jail>();


        //Methods
        /// <summary>
        /// Kicks the player
        /// </summary>
        /// <param name="message"></param>
        public void Kick(string message) => ServerConsole.Disconnect(gameObject, message);

        /// <summary>
        /// Bans the player
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="reason"></param>
        /// <param name="issuer"></param>
        public void Ban(int duration, string reason, string issuer = "Plugin") => Server.GetComponent<BanPlayer>().BanUser(gameObject, duration, reason, issuer);

        /// <summary>
        /// Kills a player
        /// </summary>
        /// <param name="damageType"></param>
        public void Kill(DamageTypes.DamageType damageType = default) => Hub.playerStats.HurtPlayer(new PlayerStats.HitInfo(-1f, "WORLD", damageType, 0), gameObject);

        [Obsolete("Does not work properly")]
        public void ChangeRoleAtPosition(RoleType role)
        {
            //TODO: Fix this shit
            Hub.characterClassManager.SetClassIDAdv(role, true);
        }

        /// <summary>
        /// Checks if the user has permission 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool CheckPermission(string permission)
        {
            if (this == Server) return true;
            try
            {
                return PermissionReader.CheckPermission(this, permission);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gives the player a text hint on his screen
        /// </summary>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void GiveTextHint(string message, float duration = 5f)
        {
            Hub.hints.Show(new TextHint(message, new HintParameter[]
                {
                    new StringHintParameter("")
                }, HintEffectPresets.FadeInAndOut(duration), duration));
        }

        /// <summary>
        /// Clear all Broadcast the Player has currently
        /// </summary>
        public void ClearBroadcasts() => GetComponent<Broadcast>().TargetClearElements(Connection);

        /// <summary>
        /// Sends a broadcast to the player
        /// </summary>
        /// <param name="time"></param>
        /// <param name="message"></param>
        public void Broadcast(ushort time,string message) => GetComponent<Broadcast>().TargetAddElement(Connection, message, time, new Broadcast.BroadcastFlags());

        /// <summary>
        /// Clear all previous broadcasts and send the broadcast instant to the player
        /// </summary>
        /// <param name="time"></param>
        /// <param name="message"></param>
        public void InstantBroadcast(ushort time, string message)
        {
            ClearBroadcasts();
            GetComponent<Broadcast>().TargetAddElement(Connection, message, time, new Broadcast.BroadcastFlags());
        }

        /// <summary>
        /// Send a message in the console of the player
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public void SendConsoleMessage(string message, string color = "red") => ClassManager.TargetConsolePrint(Connection, message, color);

        /// <summary>
        /// Sends a message in the Text based Remote Admin of the Player
        /// </summary>
        /// <param name="message"></param>
        /// <param name="success"></param>
        /// <param name="type"></param>
        public void SendRAConsoleMessage(string message, bool success = true, RaCategory type = RaCategory.None) => CommandSender.RaMessage(message, success, type);

        /// <summary>
        /// Hides for normal player the RankName the player has
        /// </summary>
        public void HideTag() => ClassManager.CallCmdRequestHideTag();

        /// <summary>
        /// Shows everyone the RankName the player has
        /// </summary>
        /// <param name="global"></param>
        public void ShowTag(bool global = false) => ClassManager.CallCmdRequestShowTag(global);

        /// <summary>
        /// Gives the player a item
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="duration"></param>
        /// <param name="sight"></param>
        /// <param name="barrel"></param>
        /// <param name="other"></param>
        public void GiveItem(ItemType itemType, float duration = float.NegativeInfinity, int sight = 0, int barrel = 0, int other = 0) => Hub.inventory.AddNewItem(itemType, duration, sight, barrel, other);

        /// <summary>
        /// Clears the players Inventory
        /// </summary>
        public void ClearInventory() => Hub.inventory.Clear();

        /// <summary>
        /// Gives the player an effect
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="intensity"></param>
        /// <param name="duration"></param>
        public void GiveEffect(Effect effect,byte intensity = 1,float duration = -1f) => EffectsController.ChangeByString(effect.ToString().ToLower(), intensity, duration);

        /// <summary>
        /// Allow the player to open the RemoteAdmin-Interface
        /// </summary>
        public void RaLogin()
        {
            Hub.serverRoles.RemoteAdmin = true;
            Hub.serverRoles.Permissions = Hub.serverRoles.Group.Permissions;
            Hub.serverRoles.RemoteAdminMode = ServerRoles.AccessMode.PasswordOverride;
            Hub.serverRoles.TargetOpenRemoteAdmin(Connection, false);
        }

        /// <summary>
        /// Denies the access to the RemoteAdmin-Interface
        /// </summary>
        public void RaLogout()
        {
            Hub.serverRoles.RemoteAdmin = false;
            Hub.serverRoles.Permissions = 0UL;
            Hub.serverRoles.RemoteAdminMode = ServerRoles.AccessMode.LocalAccess;
            Hub.serverRoles.TargetCloseRemoteAdmin(Connection);
        }

        /// <summary>
        /// Hurts the Player
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="damagetype"></param>
        /// <param name="attacker"></param>
        public void Hurt(int amount, DamageTypes.DamageType damagetype = default,Player attacker = null) =>
            PlayerStats.HurtPlayer(new PlayerStats.HitInfo(amount, attacker == null ? "WORLD" : attacker.NickName, damagetype, attacker == null ? PlayerId : attacker.PlayerId), attacker == null ? gameObject : attacker.gameObject);

        /// <summary>
        /// Sends the Player to a Server in the same network with this Port (such a server must exist or he will be disconnected)
        /// </summary>
        /// <param name="port">The Port of the Server the Player should be send to</param>
        public void SendToServer(ushort port)
        {
            PlayerStats component = Server.PlayerStats;
            NetworkWriter writer = NetworkWriterPool.GetWriter();
            writer.WriteSingle(1f);
            writer.WriteUInt16(port);
            RpcMessage msg = new RpcMessage
            {
                netId = component.netId,
                componentIndex = component.ComponentIndex,
                functionHash = PlayerExtensions.GetMethodHash(typeof(PlayerStats), "RpcRoundrestartRedirect"),
                payload = writer.ToArraySegment()
            };
            Connection.Send<RpcMessage>(msg, 0);
            NetworkWriterPool.Recycle(writer);
        }
    }
}
