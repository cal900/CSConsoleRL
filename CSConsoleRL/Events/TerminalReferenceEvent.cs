using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
  public class TerminalReferenceEvent : IGameEvent
  {
    public string EventName { get { return "PassConsoleReference"; } }
    public List<object> EventParams { get; set; }

    public TerminalReferenceEvent(List<string> terminalCommands)
    {
      EventParams = new List<object>();
      EventParams.Add(terminalCommands);
    }
  }
}
