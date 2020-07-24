using Synapse.Api.Plugin;
using System.Collections.Generic;

namespace Synapse.Config
{
    internal static class SynapseConfigs
    {
        // Configs
        internal static int RequiredForFemur;
        internal static bool RemoteKeyCard;
        internal static string JoinBroadcast;
        internal static string JoinTextHint;
        internal static ushort JoinMessageDuration;
        internal static List<int> Speaking_Scps;
        internal static bool Nametracking;

        // Methods
        internal static void ReloadConfig()
        {
            RequiredForFemur = Plugin.Config.GetInt("synapse_femur",1);
            RemoteKeyCard = Plugin.Config.GetBool("synapse_remote_keycard", false);
            JoinBroadcast = Plugin.Config.GetString("synapse_join_broadcast", "");
            JoinTextHint = Plugin.Config.GetString("synapse_join_texthint", "");
            JoinMessageDuration = Plugin.Config.GetUShort("synapse_join_duration",5);
            Speaking_Scps = Plugin.Config.GetIntList("synapse_speakingscps");
            if (Speaking_Scps == null) Speaking_Scps = new List<int> { 16, 17 };
            Nametracking = Plugin.Config.GetBool("synapse_nametracking",true);
        }
    }
}
