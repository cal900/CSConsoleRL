using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using GameTiles.Tiles;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.GameSystems
{
  public class AiSystem : GameSystem
  {
    private readonly Tile[,] _gameTiles;
    private ActorEntity _actorEntity;
    private bool _aiEnabled;
    private readonly GameStateHelper _gameStateHelper;

    public AiSystem(GameSystemManager _manager, Tile[,] _gameTiles, GameStateHelper gameStateHelper)
    {
      SystemManager = _manager;
      _systemEntities = new List<Entity>();
      this._gameTiles = _gameTiles;
      _aiEnabled = true;
      _gameStateHelper = gameStateHelper;
    }

    public override void InitializeSystem()
    {

    }

    public override void AddEntity(Entity entity)
    {
      if (entity.GetType() == typeof(ActorEntity))
      {
        _actorEntity = (ActorEntity)entity;
      }
      if (entity.Components.ContainsKey(typeof(AiComponent))
          //|| entity.Components.ContainsKey(typeof(SeekerAiComponent))
          || entity.Components.ContainsKey(typeof(IAiComponent)))
      {
        _systemEntities.Add(entity);
      }
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "NextTurn":
          NextTurn();
          break;
        case "ToggleAi":
          _aiEnabled = !_aiEnabled;
          break;
      }
    }

    private void NextTurn()
    {
      if (_aiEnabled)
      {
        foreach (var entity in _systemEntities)
        {
          GetAiResponse(entity);
        }
      }
    }

    private void SetImmediatePositionGoal(Entity ent)
    {

    }

    private void GetAiResponse(Entity ent)
    {
      var ai = ent.GetComponent<IAiComponent>();
      var resp = ai.GetAiComponentResponse(_gameStateHelper);
      if (resp != null) BroadcastMessage(resp);
    }
  }
}
