using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Events;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Entities;

namespace CSConsoleRL.GameSystems
{
    public abstract class GameSystem
    {
        public GameSystemManager SystemManager { get; set; }
        public abstract void InitializeSystem();
        public abstract void AddEntity(Entity entity);
        public abstract void HandleMessage(GameEvent gameEvent);
        public abstract GameEvent BroadcastMessage(GameEvent evnt, List<Entity> entitiesInvolved);
    }
}
