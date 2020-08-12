using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using SFML.System;

namespace CSConsoleRL.Events
{
  public class SendTargetingPathEvent : IGameEvent
  {
    public string EventName { get { return "SendTargetingPath"; } }
    public List<object> EventParams { get; set; }

    public SendTargetingPathEvent(List<Vector2i> targetingPath)
    {
      EventParams = new List<object>() { targetingPath };
    }
  }
}
