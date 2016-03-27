using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;

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

    public class MovementCommandEvent : GameEvent
    {
        public readonly string EventName = "MovementCommand";
        public List<object> EventParams;

        public MovementCommandEvent(int targetX, int targetY)
        {
            EventParams = new List<object>();
            EventParams.Add(targetX);
            EventParams.Add(targetY);
        }
    }

    //Need events for denied and each reason - e.g. collision, immobilized, etc.
    //will broadcast movement request event, then wait for any denied response, if none will broadcast move event
}
