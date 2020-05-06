using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
  public class ToggleAiEvent : IGameEvent
  {
    public string EventName { get { return "ToggleAi"; } }
    public List<object> EventParams { get; set; }

    public ToggleAiEvent()
    {

    }
  }
}
