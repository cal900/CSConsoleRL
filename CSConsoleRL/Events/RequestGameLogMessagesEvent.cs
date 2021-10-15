using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
  public class RequestGameLogMessagesEvent : IGameEvent
  {
    public string EventName { get { return "RequestGameLogMessages"; } }
    public List<object> EventParams { get; set; }

    /// <summary>
    /// Should be called by renderer to refresh GameLog data before rendering
    /// </summary>
    /// <param name="numCommands"></param>
    public RequestGameLogMessagesEvent()
    {
      EventParams = new List<object>() { };
    }
  }
}
