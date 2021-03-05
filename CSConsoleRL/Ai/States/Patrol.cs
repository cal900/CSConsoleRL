using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Helpers;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;

namespace CSConsoleRL.Ai.States
{
  public class Patrol : IAiState
  {
    private readonly Entity _entity;
    private int _counter;

    public Patrol(Entity entity)
    {
      _entity = entity;
    }

    public string GetName()
    {
      return "Patrol";
    }

    public IGameEvent GetAiStateResponse(GameStateHelper gameStateHelper)
    {
      var horMovement = 0;
      var verMovement = 0;

      _counter++;

      switch (_counter)
      {
        case 0:
          horMovement = 1;
          break;
        case 1:
          verMovement = 1;
          break;
        case 2:
          horMovement = -1;
          break;
        default:
          verMovement = -1;
          _counter = -1;
          break;
      }

      return new MovementInputEvent(_entity.Id, horMovement, verMovement);
    }
  }
}