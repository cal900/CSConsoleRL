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
        public string EventName { get { return "SnapCameraToEntity"; } }
        public List<object> EventParams { get; set; }

        public SnapCameraToEntityEvent(Entity entity)
        {
            EventParams = new List<object>();
            EventParams.Add(entity);
        }
    }
}
