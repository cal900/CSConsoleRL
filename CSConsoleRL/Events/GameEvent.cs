using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConsoleRL.Events
{
    public abstract class GameEvent
    {
        public readonly string EventName;
        public List<object> EventParams;
    }
}
