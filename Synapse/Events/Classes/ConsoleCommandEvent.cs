using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ConsoleCommandEvent
    {
        public ReferenceHub Player { get; internal set; }

        public string Command { get; internal set; }

        public string ReturnMessage { get; set; }

        public string Color { get; set; }
    }
}