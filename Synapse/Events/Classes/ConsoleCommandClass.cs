namespace Synapse.Events.Classes
{
    public class ConsoleCommandClass
    {
        public ReferenceHub Player { get; internal set; }

        public string Command { get; internal set; }

        public string ReturnMessage { get; set; }

        public string Color { get; set; }
    }
}