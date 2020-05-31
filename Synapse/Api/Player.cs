using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Synapse.Api
{
    public static class Player
    {
        /// <summary>Gives a User a Message im Remote Admin</summary>
        /// <param name="sender">The User who you send the Message</param>
        /// <param name="pluginName">The Name from which is it at the beginning of the Message</param>
        /// <param name="message">The Message you want to send</param>
        /// <param name="success">True = green the command is right you have permission and execute it successfully</param>
        /// <param name="type">In Which Category should you see it too?</param>
        public static void RaMessage(this CommandSender sender, string pluginName,string message, bool success = true, RaCategory type = RaCategory.None)
        {
            var category = "";
            switch (type)
            {
                case RaCategory.None:
                    category = "";
                    break;
                case RaCategory.PlayerInfo:
                    category = "PlayerInfo";
                    break;
                case RaCategory.ServerEvents:
                    category = "ServerEvents";
                    break;
                case RaCategory.DoorsManagement:
                    category = "DoorsManagement";
                    break;
                case RaCategory.AdminTools:
                    category = "AdminTools";
                    break;
                case RaCategory.ServerConfigs:
                    category = "ServerConfigs";
                    break;
                case RaCategory.PlayersManagement:
                    category = "PlayersManagement";
                    break;
            }

            sender.RaReply($"{pluginName}#" + message, success, true, category);
        }


        /// <summary>Sends a Broadcast to a user</summary>
        /// <param name="rh">The User you want to send a Broadcast</param>
        /// <param name="time">How Long should he see it?</param>
        /// <param name="message">The message you send</param>
        public static void Broadcast(this ReferenceHub rh, ushort time, string message) => 
            rh.GetComponent<Broadcast>().TargetAddElement(rh.scp079PlayerScript.connectionToClient, message, time, new Broadcast.BroadcastFlags());

        /// <summary>Sends a broadcast to the user and delete all previous so that he see it instantly</summary>
        /// <param name="rh">The user</param>
        /// <param name="time">How long should the new Broadcast be shown?</param>
        /// <param name="message">the broadcast message</param>
        public static void BroadcastInstant(this ReferenceHub rh, ushort time, string message)
        {
            rh.ClearBroadcasts();
            rh.Broadcast(time, message);
        }

        /// <summary>Clears all of the current Broadcast the user has</summary>
        /// <param name="player">The Player which Broadcast should be cleared</param>
        public static void ClearBroadcasts(this ReferenceHub player) => player.GetComponent<Broadcast>().TargetClearElements(player.scp079PlayerScript.connectionToClient);

        /// <returns>A List of all Players on the Server which are not the Server</returns>
        public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.Hubs.Values.Where(h => !h.isLocalPlayer);

        /// <param name="player">The User you want the Id of</param>
        /// <returns>The User ID (1234@steam) of the User</returns>
        public static string GetUserId(this ReferenceHub player) => player.characterClassManager.UserId;

        /// <summary>Gives you The Position of the User</summary>
        /// <param name="player">The User which Position you want to have</param>
        public static Vector3 GetPosition(this ReferenceHub player) => player.playerMovementSync.transform.position;

        /// <summary>Gives You the Current Room the user is in</summary>
        /// <returns></returns>
        public static Room GetCurrentRoom(this ReferenceHub player)
        {
            Vector3 playerpos = player.GetPosition();
            Vector3 end = playerpos - new Vector3(0f, 10f, 0f);
            bool flag = Physics.Linecast(playerpos, end, out RaycastHit raycastHit, -84058629);

            if (!flag || raycastHit.transform == null)
                return null;

            Transform transform = raycastHit.transform;

            while (transform.parent != null && transform.parent.parent != null)
                transform = transform.parent;

            foreach (Room room in Map.Rooms)
                if (room.Position == transform.position)
                    return room;

            return new Room
            {
                Name = transform.name,
                Position = transform.position,
                Transform = transform
            };
        }
        
        /// <summary>
        /// Gets' a players overwatch status
        /// </summary>
        /// <param name="player">The Player that needs to be checked</param>
        /// <returns type="bool">The Overwatch Status</returns>
        public static bool GetOverwatch(this ReferenceHub player) => player.serverRoles.OverwatchEnabled;

        /// <summary>
        /// Sets' a players overwatch status
        /// </summary>
        /// <param name="player">player to be modified</param>
        /// <param name="newStatus">new status to modify</param>
        public static void SetOverwatch(this ReferenceHub player, bool newStatus) =>
            player.serverRoles.SetOverwatchStatus(newStatus);
        
        
    }
}