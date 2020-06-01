using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Events.Classes
{
    public class PlayerCuffedClass
    {
        public ReferenceHub Cuffer { get; internal set; }

        public ReferenceHub Target { get; internal set; }

        public bool Allow { get; set; }
    }
}
