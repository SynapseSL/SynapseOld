using CommandSystem;
using Mirror;
using RemoteAdmin;
using System;
using System.Reflection;
using UnityEngine;

namespace Synapse.Api
{
    public static class Server
    {
        private static MethodInfo _sendSpawnMessage;
        private static Broadcast _broadcast;
        private static BanPlayer _banPlayer;


        public static Player Host => Player.Host;

        public static string Name
        {
            get => ServerConsole._serverName;
            set
            {
                ServerConsole._serverName = value;
                ServerConsole.singleton.RefreshServerName();
            }
        }

        public static ushort Port { get => ServerStatic.ServerPort; set => ServerStatic.ServerPort = value; }

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

        public static Broadcast Broadcast
        {
            get
            {
                if (_broadcast == null)
                    _broadcast = Host.GetComponent<Broadcast>();

                return _broadcast;
            }
        }

        public static BanPlayer BanPlayer
        {
            get
            {
                if (_banPlayer == null)
                    _banPlayer = Host.GetComponent<BanPlayer>();

                return _banPlayer;
            }
        }

        public static ServerConsole Console => ServerConsole.singleton;

        public static RemoteAdminCommandHandler RaCommandHandler => CommandProcessor.RemoteAdminCommandHandler;

        public static GameConsoleCommandHandler GameCoreCommandHandler => GameCore.Console.singleton.ConsoleCommandHandler;

        public static ClientCommandHandler ClientCommandHandler => QueryProcessor.DotCommandHandler;


        public static TObject[] GetObjectsOf<TObject>() where TObject : UnityEngine.Object => UnityEngine.Object.FindObjectsOfType<TObject>();

        public static int GetMethodHash(Type invokeClass, string methodName) => invokeClass.FullName.GetStableHashCode() * 503 + methodName.GetStableHashCode();
    }
}
