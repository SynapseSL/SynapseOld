using CommandSystem;
using Mirror;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Synapse.Api
{
    public static class Server
    {
        private static MethodInfo _sendSpawnMessage;
        private static Broadcast _broadcast;
        private static BanPlayer _banPlayer;

        /// <summary>
        /// Gives you the Player object of the Server
        /// </summary>
        public static Player Host => Player.Host;

        /// <summary>
        /// Get/Sets the ServerName
        /// </summary>
        public static string Name
        {
            get => ServerConsole._serverName;
            set
            {
                ServerConsole._serverName = value;
                ServerConsole.singleton.RefreshServerName();
            }
        }

        /// <summary>
        /// Get/Sets the Port of the Server
        /// </summary>
        public static ushort Port { get => ServerStatic.ServerPort; set => ServerStatic.ServerPort = value; }

        /// <summary>
        /// SpawnMessage MethodInfo
        /// </summary>
        public static MethodInfo SendSpawnMessage
        {
            get
            {
                if (_sendSpawnMessage == null)
                    _sendSpawnMessage = typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.Instance | BindingFlags.InvokeMethod
                        | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);

                return _sendSpawnMessage;
            }
        }

        /// <summary>
        /// The Broadcast object of the Server
        /// </summary>
        public static Broadcast Broadcast
        {
            get
            {
                if (_broadcast == null)
                    _broadcast = Host.GetComponent<Broadcast>();

                return _broadcast;
            }
        }

        /// <summary>
        /// The BanPlayer object of the Server
        /// </summary>
        public static BanPlayer BanPlayer
        {
            get
            {
                if (_banPlayer == null)
                    _banPlayer = Host.GetComponent<BanPlayer>();

                return _banPlayer;
            }
        }

        /// <summary>
        /// Gives you the ServerConsole
        /// </summary>
        public static ServerConsole Console => ServerConsole.singleton;

        /// <summary>
        /// The RemoteAdmin Command Handler
        /// </summary>
        /// <remarks>
        /// You can use it to register Commands
        /// </remarks>
        public static RemoteAdminCommandHandler RaCommandHandler => CommandProcessor.RemoteAdminCommandHandler;

        /// <summary>
        /// The ServerConsole Command Handler
        /// </summary>
        /// <remarks>
        /// You can use it to register Commands
        /// </remarks>
        public static GameConsoleCommandHandler GameCoreCommandHandler => GameCore.Console.singleton.ConsoleCommandHandler;

        /// <summary>
        /// The Client Command Handler
        /// </summary>
        /// <remarks>
        /// You can use it to register Commands
        /// </remarks>
        public static ClientCommandHandler ClientCommandHandler => QueryProcessor.DotCommandHandler;

        /// <summary>
        /// Gives you a list of all objects with this Type
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static List<TObject> GetObjectsOf<TObject>() where TObject : UnityEngine.Object => UnityEngine.Object.FindObjectsOfType<TObject>().ToList();

        public static TObject GetObjectOf<TObject>() where TObject : UnityEngine.Object => UnityEngine.Object.FindObjectOfType<TObject>();

        /// <summary>
        /// Gives you the MethodHash
        /// </summary>
        /// <param name="invokeClass"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static int GetMethodHash(Type invokeClass, string methodName) => invokeClass.FullName.GetStableHashCode() * 503 + methodName.GetStableHashCode();
    }
}
