using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Events.Classes
{
    public class FemurEnterEvent
    {
        public ReferenceHub Player { get; internal set; }

        public bool Allow { get; set; }

        public bool CloseFemur { get; set; }
    }
}
