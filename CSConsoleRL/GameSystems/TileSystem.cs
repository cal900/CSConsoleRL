using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Tiles;

namespace CSConsoleRL.GameSystems
{
    public class TileSystem : GameSystem
    {
        public BaseTile[,] TileSet;

        public TileSystem(GameSystemManager manager)
        {
            SystemManager = manager;

            TileSet = new BaseTile[100, 100];
        }

        public override void AddComponent(IComponent component)
        {

        }

        public void HandleMessage(GameEvent gameEvent)
        {

        }
    }
}
