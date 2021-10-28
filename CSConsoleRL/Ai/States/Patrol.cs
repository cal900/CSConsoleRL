using System.Collections.Generic;
using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Helpers;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using SFML.System;
using GameTiles.Tiles;
using CSConsoleRL.Components;

namespace CSConsoleRL.Ai.States
{
  public class Patrol : IAiState
  {
    private readonly Entity _entity;
    private readonly List<Vector2i> _patrolPoints;
    private int _patrolPointIndex;
    private List<Vector2i> _path;
    private int _counter;

    public Patrol(Entity entity, List<Vector2i> patrolPoints)
    {
      _entity = entity;
      _patrolPoints = patrolPoints;
    }

    private void IncrPatrolPointIndex()
    {
      _patrolPointIndex++;
      if (_patrolPointIndex >= _patrolPoints.Count)
      {
        _patrolPointIndex = 0;
      }
    }

    public string GetName()
    {
      return "Patrol";
    }

    public IGameEvent GetAiStateResponse(GameStateHelper gameStateHelper)
    {
      var map = gameStateHelper.GetVar<Tile[,]>("GameTiles");
      var position = _entity.GetComponent<PositionComponent>();
      var currentPatrolPoint = _patrolPoints[_patrolPointIndex];
      int horMovement = 0, verMovement = 0;

      // Have we reached the current patrol point? Then move on to the next one
      if (position.ComponentXPositionOnMap == currentPatrolPoint.X
        && position.ComponentYPositionOnMap == currentPatrolPoint.Y)
      {
        IncrPatrolPointIndex();
        currentPatrolPoint = _patrolPoints[_patrolPointIndex];
      }

      // Call to A* Pathfinding to get path
      var path = PathfindingHelper.Instance.Path(map, new Vector2i(position.ComponentXPositionOnMap, position.ComponentYPositionOnMap),
        new Vector2i(currentPatrolPoint.X, currentPatrolPoint.Y));
      var nextPos = path[0];

      // Move to desired location
      if (position.ComponentXPositionOnMap < nextPos.X)
      {
        horMovement++;
      }
      else if (position.ComponentXPositionOnMap > nextPos.X)
      {
        horMovement--;
      }
      if (position.ComponentYPositionOnMap < nextPos.Y)
      {
        verMovement++;
      }
      else if (position.ComponentYPositionOnMap > nextPos.Y)
      {
        verMovement--;
      }

      return new MovementInputEvent(_entity.Id, horMovement, verMovement);
    }
  }
}
