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
    public class ConsoleKeyPressedEvent : IGameEvent
    {
        public string EventName { get { return "ConsoleKeyPressed"; } }
        public List<object> EventParams { get; set; }

        public ConsoleKeyPressedEvent(Keyboard.Key keyPressed)
        {
            EventParams = new List<object>();
            EventParams.Add(keyPressed);
        }
    }
}
