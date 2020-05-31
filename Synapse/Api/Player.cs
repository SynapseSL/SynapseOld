using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Api
{
    public static class Player
    {
		/// <summary>Gives a User a Message im Remote Admin</summary>
		/// <param name="sender">The User who you send the Message</param>
		/// <param name="pluginname">The Name from which is it at the begining of the Meessage</param>
		/// <param name="message">The Message you want to send</param>
		/// <param name="success">True = green the command is right you have permission and execute it sucessfully</param>
		/// <param name="Type">In Which Category should you see it too?</param>
		public static void RAMessage(this CommandSender sender, string pluginname,string message, bool success = true, RaCategory Type = RaCategory.None)
		{
			string category = "";
			if (Type == RaCategory.None) category = "";
			if (Type == RaCategory.Playerinfo) category = "PlayerInfo";
			if (Type == RaCategory.ServerEvents) category = "ServerEvents";
			if (Type == RaCategory.DoorsManagement) category = "DoorsManagement";
			if (Type == RaCategory.AdminTools) category = "AdminTools";
			if (Type == RaCategory.ServerConfigs) category = "ServerConfigs";
			if (Type == RaCategory.PlayersManamgement) category = "PlayersManagement";

			sender.RaReply($"{pluginname}#" + message, success, true, category);
		}


		/// <summary>Sends a Broadcast to a user</summary>
		/// <param name="rh">The User you want to send a Broadcast</param>
		/// <param name="time">How Long should he see it?</param>
		/// <param name="message">The message you send</param>
		public static void Broadcast(this ReferenceHub rh, ushort time, string message) => 
			rh.GetComponent<Broadcast>().TargetAddElement(rh.scp079PlayerScript.connectionToClient, message, time, new global::Broadcast.BroadcastFlags());

		/// <summary>Sends a broadcast to the user and delete all previous so that he see it instantly</summary>
		/// <param name="rh">The user</param>
		/// <param name="time">How long should the new Broadcast be shwon?</param>
		/// <param name="message">the broadcast message</param>
		public static void Broadcastinstant(this ReferenceHub rh, ushort time, string message)
		{
			rh.ClearBroadcasts();
			rh.Broadcast(time, message);
		}

		/// <summary>Clears all of the current Broadcast the user has</summary>
		/// <param name="player">The Player whicht Broadcast should be cleared</param>
		public static void ClearBroadcasts(this ReferenceHub player) => player.GetComponent<Broadcast>().TargetClearElements(player.scp079PlayerScript.connectionToClient);

		/// <returns>A List of all Players on the Server which are not the Server</returns>
		public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.Hubs.Values.Where(h => !h.isLocalPlayer);

		/// <param name="player">The User you want the Id of</param>
		/// <returns>The User ID (1234@steam) of the User</returns>
		public static string GetUserId(this ReferenceHub player) => player.characterClassManager.UserId;
	}
}
