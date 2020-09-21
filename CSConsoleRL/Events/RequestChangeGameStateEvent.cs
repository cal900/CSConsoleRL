using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.Events
{
  public class RequestChangeGameStateEvent : IGameEvent
  {
    public string EventName { get { return "RequestChangeGameState"; } }
    public List<object> EventParams { get; set; }

    public RequestChangeGameStateEvent(EnumGameState enumGameState)
    {
      EventParams = new List<object>() { };
      EventParams.Add(enumGameState);
    }
  }
}
