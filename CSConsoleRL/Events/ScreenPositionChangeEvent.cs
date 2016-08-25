using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
    public class ScreenPositionChangeEvent : GameEvent
    {
        public readonly string EventName = "ScreenPositionChange";
        public List<object> EventParams;

        public ScreenPositionChangeEvent(int newX, int newY)
        {
            EventParams.Add(newX);
            EventParams.Add(newY);
        }
    }
}
