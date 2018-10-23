using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
    public class RequestConsoleDataEvent : IGameEvent
    {
        public string EventName { get { return "RequestConsoleData"; } }
        public List<object> EventParams { get; set; }

        /// <summary>
        /// We pass in numCommands to indicate how many commands/lines of the console is displayed at once (to avoid passing entire command history)
        /// </summary>
        /// <param name="numCommands"></param>
        public RequestConsoleDataEvent(int numCommands)
        {
            EventParams = new List<object>() { numCommands };
        }
    }
}
