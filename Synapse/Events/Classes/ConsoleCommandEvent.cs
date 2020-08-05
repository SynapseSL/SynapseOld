using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ConsoleCommandEvent
    {
        public Player Player { get; internal set; }

        public string Command { get; internal set; }
    }
}