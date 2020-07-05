using System.IO;

namespace Synapse
{
    internal static class Configs
    {
        // Configs
        internal static int RequiredForFemur;
        internal static bool RemoteKeyCard;
        internal static string JoinBroadcast;
        internal static string JoinTextHint;
        internal static ushort JoinMessageDuration;

        // Methods
        internal static void ReloadConfig()
        {
            RequiredForFemur = Plugin.Config.GetInt("synapse_femur",1);
            RemoteKeyCard = Plugin.Config.GetBool("synapse_remote_keycard", false);
            JoinBroadcast = Plugin.Config.GetString("synapse_join_broadcast", "");
            JoinTextHint = Plugin.Config.GetString("synapse_join_texthint", "");
            JoinMessageDuration = Plugin.Config.GetUShort("synapse_join_duration",5);
        }
    }
}
