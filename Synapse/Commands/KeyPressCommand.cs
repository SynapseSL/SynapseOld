using CommandSystem;
using Synapse.Api;
using System;
using System.Linq;
using UnityEngine;

namespace Synapse.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class KeyPressCommand : ICommand
    {
        public string Command { get; } = "keypress";

        public string[] Aliases { get; } = new string[]
        {
            "kp",
            "key",
            "keybind"
        };

        public string Description { get; } = "A Command for the KeyPressEvent from Synapse!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string respone)
        {
            if (sender.GetPlayer() == Server.Host)
            {
                respone = "Nope the Console cant use this!";
                return false;
            }

            switch (arguments.FirstOrDefault().ToUpper())
            {
                case "SYNC":
                    var component = sender.GetPlayer().ClassManager;
                    foreach(var key in (KeyCode[])Enum.GetValues(typeof(KeyCode)))
                        component.TargetChangeCmdBinding(component.connectionToClient, key, $".key send {(int)key}");

                    respone = "All Keys was synced!";
                    return true;

                case "SEND":
                    if(!Enum.TryParse<KeyCode>(arguments.ElementAt(1), out var key2))
                    {
                        respone = "Invalid KeyBind! If they are binded by Synapse please report this!";
                        return false;
                    }

                    try
                    {
                        Events.Events.InvokeKeyPressEvent(sender.GetPlayer(), key2);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"KeyPressEvent Error: {e} ");
                    }
                    respone = "Key was accepted";
                    return true;
                default:
                    respone = "Use .key sync in order to sync your binds and use all Features of the Plugins!";
                    return false;
            }
        }
    }
}
