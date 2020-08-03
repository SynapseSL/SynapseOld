namespace Synapse.Api
{
    public static class Warhead
    {
        private static AlphaWarheadController _controller;
        private static AlphaWarheadNukesitePanel _nukeSitePanel;

        public static AlphaWarheadController Controller
        {
            get
            {
                if (_controller == null)
                    _controller = Player.Host.GetComponent<AlphaWarheadController>();

                return _controller;
            }
        }

        public static AlphaWarheadNukesitePanel NukeSitePanel
        {
            get
            {
                if (_nukeSitePanel == null)
                    _nukeSitePanel = Player.Host.GetComponent<AlphaWarheadNukesitePanel>();

                return _nukeSitePanel;
            }
        }

        public static bool LeverStatus { get => NukeSitePanel.Networkenabled; set => NukeSitePanel.Networkenabled = value; }

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
