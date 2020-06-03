using System;
using System.Diagnostics.CodeAnalysis;
using Assets._Scripts.Dissonance;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SpeakEventClass : EventArgs
    {
        public ReferenceHub Player { get; internal set; }

        public DissonanceUserSetup DissonanceUserSetup { get; internal set; }

        public bool Scp939Talk { get; set; }

        public bool IntercomTalk { get; set; }

        public bool RadioTalk { get; set; }

        public bool ScpChat { get; set; }

        public bool SpectatorChat { get; set; }
    }
}