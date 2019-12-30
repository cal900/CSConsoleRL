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
    public class SnapCameraToEntityEvent : IGameEvent
    {
        public string EventName { get { return "MovementInput"; } }
        public List<object> EventParams { get; set; }

        public SnapCameraToEntityEvent(Guid id, int movementX, int movementY)
        {
            EventParams = new List<object>();
            EventParams.Add(id);
            EventParams.Add(movementX);
            EventParams.Add(movementY);
        }
    }
}
