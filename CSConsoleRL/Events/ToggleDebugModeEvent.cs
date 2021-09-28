using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
  public class ToggleDebugModeEvent : IGameEvent
  {
    public string EventName { get { return "ToggleDebugMode"; } }
    public List<object> EventParams { get; set; }

    public ToggleDebugModeEvent(bool? debugMode = null)
    {
      EventParams = new List<object>();
      if (debugMode != null) EventParams.Add(debugMode);
    }
  }
}
