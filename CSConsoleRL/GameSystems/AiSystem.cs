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
    private Tile[,] _gameTiles;
    private ActorEntity _actorEntity;
    private bool _aiEnabled;

    public AiSystem(GameSystemManager _manager, Tile[,] _gameTiles)
    {
      SystemManager = _manager;
      _systemEntities = new List<Entity>();
      this._gameTiles = _gameTiles;
      _aiEnabled = true;
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
          || entity.Components.ContainsKey(typeof(SeekerAiComponent)))
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

      GetSeekerResponse(ent);
    }

    private void GetSeekerResponse(Entity ent)
    {
      var seekerAi = ent.GetComponent<SeekerAiComponent>();
      var position = ent.GetComponent<PositionComponent>();
      int horMovement = 0, verMovement = 0;

      //Call to A* Pathfinding to get path
      var path = PathfindingHelper.Instance.Path(_gameTiles, new Vector2i(position.ComponentXPositionOnMap, position.ComponentYPositionOnMap),
          new Vector2i(_actorEntity.GetComponent<PositionComponent>().ComponentXPositionOnMap, _actorEntity.GetComponent<PositionComponent>().ComponentYPositionOnMap));

      if (path != null)
      {
        if (path != null) seekerAi.Path = path;
        // if (path[1] != null) seekerAi.AnalyzedPath = path[1];

        //For debugging color in path and analyzed tiles
        // foreach (var tile in path[1])
        // {
        //   var fadingColorEnt = new FadingColorEntity("yellow");
        //   fadingColorEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = tile.X;
        //   fadingColorEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = tile.Y;
        //   SystemManager.RegisterEntity(fadingColorEnt);
        // }
        // foreach (var tile in path)
        // {
        //   var fadingColorEnt = new FadingColorEntity("green");
        //   fadingColorEnt.GetComponent<PositionComponent>().ComponentXPositionOnMap = tile.X;
        //   fadingColorEnt.GetComponent<PositionComponent>().ComponentYPositionOnMap = tile.Y;
        //   SystemManager.RegisterEntity(fadingColorEnt);
        // }
      }

      //Move to desired location
      if (position.ComponentXPositionOnMap < seekerAi.DesiredX)
      {
        horMovement++;
      }
      else if (position.ComponentXPositionOnMap > seekerAi.DesiredX)
      {
        horMovement--;
      }
      if (position.ComponentYPositionOnMap < seekerAi.DesiredY)
      {
        verMovement++;
      }
      else if (position.ComponentYPositionOnMap > seekerAi.DesiredY)
      {
        verMovement--;
      }

      BroadcastMessage(new MovementInputEvent(ent.Id, horMovement, verMovement));
    }
  }
}
