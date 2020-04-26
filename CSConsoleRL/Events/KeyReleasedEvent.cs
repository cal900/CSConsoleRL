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
  public class KeyReleasedEvent : IGameEvent
  {
    public string EventName { get { return "KeyReleased"; } }
    public List<object> EventParams { get; set; }

    public KeyReleasedEvent(Keyboard.Key keyPressed)
    {
      EventParams.Add(keyPressed);
    }
  }
}
