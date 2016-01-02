using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Events;
using CSConsoleRL.Components.Interfaces;

namespace CSConsoleRL.GameSystems
{
    public abstract class GameSystem
    {
        public GameSystemManager SystemManager { get; set; }
        public void AddComponent(IComponent component); //New components are casted when added rather than when called, implement list in inherting Systems
        public abstract void HandleMessage(GameEvent evt);
    }
}
