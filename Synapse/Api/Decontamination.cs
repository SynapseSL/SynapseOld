using LightContainmentZoneDecontamination;
using Mirror;

namespace Synapse.Api
{
    public static class Decontamination
    {
        /// <summary>
        /// Gives you the Decontamination Controller
        /// </summary>
        public static DecontaminationController Controller => DecontaminationController.Singleton;

        /// <summary>
        /// Is the Decontamination Countdown disabled?
        /// </summary>
        public static bool IsDecontaminationDisabled 
        { 
            get => Controller._disableDecontamination; 
            set
            {
                if (value)
                {
                    Controller._stopUpdating = false;
                    Controller.RoundStartTime = NetworkTime.time;
                }
                Controller._disableDecontamination = value;
            }
        }

        /// <summary>
        /// Is the Decontamination in Progress?
        /// </summary>
        public static bool IsDecontaminationInProgress => Controller._decontaminationBegun;

        /// <summary>
        ///  Starts the Decontamination
        /// </summary>
        public static void StartDecontamination() => Controller.FinishDecontamination();
    }
}
