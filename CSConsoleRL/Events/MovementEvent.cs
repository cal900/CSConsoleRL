using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using SFML.Window;

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

    public class MovementEvent : GameEvent
    {
        public readonly string EventName = "Movement";
        public List<object> EventParams;

        public MovementEvent(Guid id, EnumDirections movementDirection)
        {
            EventParams.Add(id);
            EventParams.Add(movementDirection);
        }
    }
}
