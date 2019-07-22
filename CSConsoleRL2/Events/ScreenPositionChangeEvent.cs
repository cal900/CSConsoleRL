using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
    public class ScreenPositionChangeEvent : IGameEvent
    {
        public string EventName { get { return "ScreenPositionChange"; } }
        public List<object> EventParams { get; set; }

        public ScreenPositionChangeEvent(int newX, int newY)
        {
            EventParams = new List<object>();
            EventParams.Add(newX);
            EventParams.Add(newY);
        }
    }
}
