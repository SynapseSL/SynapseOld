using LightContainmentZoneDecontamination;
using Mirror;

namespace Synapse.Api
{
    public static class Decontamination
    {
        public static DecontaminationController Controller { get => DecontaminationController.Singleton; }

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

        public static bool IsDecontaminationInProgress => Controller._decontaminationBegun;

        /// <summary>
        ///  Starts the Decontamination
        /// </summary>
        public static void StartDecontamination() => Controller.FinishDecontamination();
    }
}
