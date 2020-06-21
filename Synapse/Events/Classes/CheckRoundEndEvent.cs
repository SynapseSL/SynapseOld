namespace Synapse.Events.Classes
{
    public class CheckRoundEndEvent
    {
        public RoundSummary.LeadingTeam LeadingTeam { get; set; }
        public bool ForceEnd { get; set; }
        public bool Allow { get; set; }
    }
}