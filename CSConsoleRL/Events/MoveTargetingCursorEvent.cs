using System;
using System.Collections.Generic;
using CSConsoleRL.Enums;

namespace CSConsoleRL.Events
{
  public class MoveTargetingCursorEvent : IGameEvent
  {
    public string EventName { get { return "MoveTargetingCursor"; } }
    public List<object> EventParams { get; set; }

    public MoveTargetingCursorEvent(EnumDirections dir)
    {
      EventParams = new List<object>();
      EventParams.Add(dir);
    }
  }
}
