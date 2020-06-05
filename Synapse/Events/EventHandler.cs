using System.Collections.Generic;
using MEC;
using Synapse.Api;
using Synapse.Events.Classes;
using Synapse.Permissions;
using UnityEngine;

namespace Synapse.Events
{
    // ReSharper disable once UnusedType.Global
    internal class EventHandler
    {
        // Variables
        private bool _roundInProgress;

        // ReSharper disable once NotAccessedField.Local
        private int _roundTime;

        // Constructor
        public EventHandler()
        {
            Events.SyncDataEvent += OnSyncData;
            Events.RemoteCommandEvent += OnRemoteCommand;
            Events.RoundStartEvent += OnRoundStart;
            Events.RoundEndEvent += OnRoundEnd;
            Events.RoundRestartEvent += OnRoundRestart;
        }

        // Methods
        private static void OnSyncData(ref SyncDataEvent ev)
        {
            if (ev.Player.GetRole() != RoleType.ClassD &&
                ev.Player.GetRole() != RoleType.Scientist &&
                !(Vector3.Distance(ev.Player.GetPosition(), ev.Player.GetComponent<Escape>().worldPosition) >= Escape.radius))
                ev.Player.characterClassManager.CmdRegisterEscape();
        }

        private static void OnRemoteCommand(ref RemoteCommandEvent ev)
        {
            var args = ev.Command.Split(' ');
            switch (args[0].ToUpper())
            {
                case "RELOADPERMISSION":
                {
                    if (!ev.Player.IsAllowed("sy.reload"))
                    {
                        ev.Sender.RaMessage("Synapse", "You have no Permission for Reload", false,
                            RaCategory.AdminTools);
                        return;
                    }

                    PermissionReader.ReloadPermission();
                    ev.Sender.RaMessage("Synapse", "Permissions Reloaded!", true, RaCategory.AdminTools);
                    return;
                }
            }
        }

        private void OnRoundStart()
        {
            Timing.RunCoroutine(RoundTime());
            _roundInProgress = true;
        }

        private void OnRoundEnd()
        {
            _roundInProgress = false;
        }

        private void OnRoundRestart()
        {
            _roundInProgress = false;
            Map.Rooms.Clear();
        }

        private IEnumerator<float> RoundTime()
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(1f);
                _roundTime++;
                if (!_roundInProgress) break;
            }
        }
    }
}