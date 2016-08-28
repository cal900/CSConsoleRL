using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSConsoleRL.Events
{
    public interface IGameEvent
    {
        string EventName { get; }
        List<object> EventParams { get; set; }
    }
}
