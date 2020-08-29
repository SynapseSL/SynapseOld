using UnityEngine;

namespace Synapse.Api
{
    public class Scp106Controller : MonoBehaviour
    {
        private Player player => this.GetPlayer();
        private Scp106PlayerScript script => player.ClassManager.Scp106;

        public bool Is106 => player.Role == RoleType.Scp106;

        public Vector3 PortalPosition { get => script.NetworkportalPosition; set => script.SetPortalPosition(value); }


        public void UsePortal() => script.UseTeleport();

        public void DeletePortal() => script.DeletePortal();

        public void CreatePortal() => script.CreatePortalInCurrentPosition();

        public void Contain() => script.Contain(player.Hub);

        public void CapturePlayer(Player player) => script.CallCmdMovePlayer(player.gameObject, ServerTime.time);
    }
}
