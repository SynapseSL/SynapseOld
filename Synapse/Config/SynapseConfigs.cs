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
        internal static List<int> SpeakingScps;
        internal static bool NameTracking;

        // Methods
        internal static void ReloadConfig()
        {
            RequiredForFemur = Plugin.Config.GetInt("synapse_femur",1);
            RemoteKeyCard = Plugin.Config.GetBool("synapse_remote_keycard");
            JoinBroadcast = Plugin.Config.GetString("synapse_join_broadcast");
            JoinTextHint = Plugin.Config.GetString("synapse_join_texthint");
            JoinMessageDuration = Plugin.Config.GetUShort("synapse_join_duration",5);
            SpeakingScps = Plugin.Config.GetIntList("synapse_speakingscps") ?? new List<int> { 16, 17 };
            NameTracking = Plugin.Config.GetBool("synapse_nametracking",true);
        }
    }
}
