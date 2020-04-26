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
  public class KeyPressedEvent : IGameEvent
  {
    public string EventName { get { return "KeyPressed"; } }
    public List<object> EventParams { get; set; }

    public KeyPressedEvent(Keyboard.Key keyPressed)
    {
      EventParams = new List<object>();
      EventParams.Add(keyPressed);
    }
  }
}
