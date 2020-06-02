using MEC;
using Synapse.Api;
using Synapse.Events.Classes;
using Synapse.Permissions;
using System.Collections.Generic;
using UnityEngine;

namespace Synapse.Events
{
    internal class EventHandler
    {
        //Variablen
        internal int roundtime;
        internal bool roundinprogress = false;

        //Konstruktor
        public EventHandler()
        {
            //SyncDataEvent hooken
            Events.RemoteCommandEvent += OnRemoteCommand;
            Events.RoundStartEvent += OnRoundStart;
            Events.RoundEndEvent += OnRoundEnd;
            Events.RoundRestartEvent += OnRoundRestart;
        }

        //Methoden
        internal void OnSyncData(SyncDataClass ev)
        {
            //ev.Player
            ReferenceHub player = Player.GetPlayer("");

            if (player.GetCurrentRoom().Zone == ZoneType.Surface && player.GetRole() != RoleType.ClassD && player.GetRole() != RoleType.Scientist &&
                !(Vector3.Distance(player.GetPosition(), player.GetComponent<Escape>().worldPosition) >= (float)(Escape.radius * 2)))
                player.characterClassManager.CmdRegisterEscape();
        }

        internal void OnRemoteCommand(ref RemoteCommandClass ev)
        {
            string[] args = ev.Command.Split(' ');
            switch (args[0].ToUpper())
            {
                case "RELOADPERMISSION":
                    {
                        if (!ev.Player.IsAllowed("sy.reload"))
                        {
                            ev.Sender.RaMessage("Synapse", "You have no Permission for Reload",false,RaCategory.AdminTools);
                            return;
                        }
                        PermissionReader.ReloadPermission();
                        ev.Sender.RaMessage("Synapse","Permissions Reloaded!",true,RaCategory.AdminTools);
                        return;
                    }
            }
        }

        internal void OnRoundStart()
        {
            Timing.RunCoroutine(Roundtime());
            roundinprogress = true;
        }

        internal void OnRoundEnd() => roundinprogress = false;

        internal void OnRoundRestart()
        {
            roundinprogress = false;
            Map.Rooms.Clear();
        }

        internal IEnumerator<float> Roundtime()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1f);
                roundtime++;
                if (!roundinprogress) break; 
            }
        }
    }
}
