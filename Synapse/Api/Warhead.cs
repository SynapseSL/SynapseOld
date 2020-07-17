namespace Synapse.Api
{
    public static class Warhead
    {
        private static AlphaWarheadController controller;
        private static AlphaWarheadNukesitePanel nukesitePanel;

        public static AlphaWarheadController Controller
        {
            get
            {
                if (controller == null)
                    controller = Player.Host.GetComponent<AlphaWarheadController>();

                return controller;
            }
        }

        public static AlphaWarheadNukesitePanel NukesitePanel
        {
            get
            {
                if (nukesitePanel == null)
                    nukesitePanel = Player.Host.GetComponent<AlphaWarheadNukesitePanel>();

                return nukesitePanel;
            }
        }

        public static bool LeverStatus { get => NukesitePanel.Networkenabled; set => NukesitePanel.Networkenabled = value; }

        public static float DetonationTimer
        {
            get => Controller.NetworktimeToDetonation;
            set => Controller.NetworktimeToDetonation = value;
        }

        /// <summary>
        /// Is the nuke detonated?
        /// </summary>
        public static bool IsNukeDetonated => Controller.detonated;

        /// <summary>
        /// Is the nuke in progress?
        /// </summary>
        public static bool IsNukeInProgress => Controller.inProgress;

        /// <summary>
        /// Starts the nuke
        /// </summary>
        public static void StartNuke()
        {
            Controller.InstantPrepare();
            Controller.StartDetonation();
        }

        /// <summary>
        /// Stops the nuke
        /// </summary>
        public static void StopNuke() => Controller.CancelDetonation();

        /// <summary>
        /// Detonates the nuke
        /// </summary>
        public static void DetonateNuke() => Controller.Detonate();

        public static void Shake() => Controller.RpcShake(true);
    }
}
