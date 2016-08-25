using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
    //public class MovementRequestEvent : GameEvent
    //{
    //    public readonly string EventName = "MovementRequest";
    //    public List<object> EventParams;

    //    public MovementRequestEvent(EnumDirections direction)
    //    {
    //        EventParams = new List<object>();
    //        EventParams.Add(direction);
    //    }
    //}

    public class NextFrameEvent : GameEvent
    {
        public readonly string EventName = "NextFrame";
        public List<object> EventParams;

        public NextFrameEvent()
        {

        }
    }
}
