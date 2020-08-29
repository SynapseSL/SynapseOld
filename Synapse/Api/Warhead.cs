namespace Synapse.Api
{
    public static class Warhead
    {
        private static AlphaWarheadController _controller;
        private static AlphaWarheadNukesitePanel _nukeSitePanel;
        private static AlphaWarheadOutsitePanel _outsidepanel;

        /// <summary>
        /// Gives you the Decontamination Controller
        /// </summary>
        public static AlphaWarheadController Controller
        {
            get
            {
                if (_controller == null)
                    _controller = Player.Host.GetComponent<AlphaWarheadController>();

                return _controller;
            }
        }

        /// <summary>
        /// Gives you the NukesiktePanel
        /// </summary>
        public static AlphaWarheadNukesitePanel NukeSitePanel
        {
            get
            {
                if (_nukeSitePanel == null)
                    _nukeSitePanel = Server.GetObjectOf<AlphaWarheadNukesitePanel>();

                return _nukeSitePanel;
            }
        }

        public static AlphaWarheadOutsitePanel OutsitePanel
        {
            get
            {
                if (_outsidepanel == null)
                    _outsidepanel = Server.GetObjectOf<AlphaWarheadOutsitePanel>();

                return _outsidepanel;
            }
        }

        public static bool Enabled { get => NukeSitePanel.Networkenabled; set => NukeSitePanel.Networkenabled = value; }

        /// <summary>
        /// Get / Set the LeverStatus
        /// </summary>
        public static bool LeverStatus { get => NukeSitePanel.Networkenabled; set => NukeSitePanel.Networkenabled = value; }

        /// <summary>
        /// The Time to Detonation
        /// </summary>
        public static float DetonationTimer
        {
            get => Controller.NetworktimeToDetonation;
            set => Controller.NetworktimeToDetonation = value;
        }

        /// <summary>
        /// Is the Nuke Detonated?
        /// </summary>
        public static bool IsNukeDetonated => Controller.detonated;

        /// <summary>
        /// Is the Nuke in Progress?
        /// </summary>
        public static bool IsNukeInProgress => Controller.inProgress;

        /// <summary>
        /// Starts the Nuke
        /// </summary>
        public static void StartNuke()
        {
            Controller.InstantPrepare();
            Controller.StartDetonation();
        }

        /// <summary>
        /// Stops the Nuke
        /// </summary>
        public static void StopNuke() => Controller.CancelDetonation();

        /// <summary>
        /// Detonates the Nuke
        /// </summary>
        public static void DetonateNuke() => Controller.Detonate();

        /// <summary>
        /// Shakes the Screen for all player like when the Nuke explodes
        /// </summary>
        public static void Shake() => Controller.RpcShake(true);
    }
}
