using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using CSConsoleRL.Data;

namespace CSConsoleRL.Events
{
  public class AddGameLogMessageEvent : IGameEvent
  {
    public string EventName { get { return "AddGameLogMessage"; } }
    public List<object> EventParams { get; set; }

    /// <summary>
    /// Adds new message string to top of GameLog
    /// </summary>
    /// <param name="entity"></param>
    public AddGameLogMessageEvent(string message)
    {
      EventParams = new List<object>() { message };
    }
  }
}
