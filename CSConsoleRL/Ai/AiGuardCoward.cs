using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Entities;
using CSConsoleRL.Helpers;
using CSConsoleRL.Ai.States;
using CSConsoleRL.Events;

namespace CSConsoleRL.Ai
{
  public class AiGuardCoward : IAi
  {
    protected readonly Entity _entity;
    protected readonly AiStateMachine _aiStateMachine;
    protected int _counter;

    protected bool Patrol1ToMeleeSeek1(Entity entity, GameStateHelper gameStateHelper)
    {
      return true;
      _counter++;
      if (_counter >= 5)
      {
        return true;
      }

      return false;
    }

    protected bool MeleeSeek1ToPatrol1(Entity entity, GameStateHelper gameStateHelper)
    {
      return false;
      _counter--;
      if (_counter <= 0)
      {
        return true;
      }

      return false;
    }

    public AiGuardCoward(Entity entity)
    {
      _entity = entity;
      _aiStateMachine = new AiStateMachine();

      ConstructAiStateMachine();
    }

    public void ParseEntity()
    {

    }

    public IGameEvent GetAiResponse(GameStateHelper gameStateHelper)
    {
      return _aiStateMachine.GetCurrentStateResponse(_entity, gameStateHelper);
    }

    public void ConstructAiStateMachine()
    {
      _aiStateMachine.AddState("Patrol1", new Patrol(_entity));
      _aiStateMachine.AddState("MeleeSeek1", new MeleeSeek(_entity));

      _aiStateMachine.AddStateChange("Patrol1", "MeleeSeek1", Patrol1ToMeleeSeek1);
      _aiStateMachine.AddStateChange("MeleeSeek1", "Patrol1", MeleeSeek1ToPatrol1);
    }
  }
}