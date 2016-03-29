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

    public class MovementInputEvent : GameEvent
    {
        public readonly string EventName = "MovementInput";
        public List<object> EventParams;

        public MovementInputEvent(EnumDirections directionOfInput)
        {
            EventParams.Add(directionOfInput);
        }
    }
}
