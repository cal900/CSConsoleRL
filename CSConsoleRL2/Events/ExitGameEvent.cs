using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
    public class ExitGameEvent : IGameEvent
    {
        public string EventName { get { return "ExitGame"; } }
        public List<object> EventParams { get; set; }

        public ExitGameEvent()
        {

        }
    }
}
