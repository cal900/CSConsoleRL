using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;

namespace CSConsoleRL.Events
{
  public class SendGameLogMessagesEvent : IGameEvent
  {
    public string EventName { get { return "SendGameLogMessages"; } }
    public List<object> EventParams { get; set; }

    /// <summary>
    /// Should be called after RequestGameLogMessages
    /// </summary>
    public SendGameLogMessagesEvent(List<string> msgs)
    {
      EventParams = new List<object>() { msgs };
    }
  }
}
