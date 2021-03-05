using CSConsoleRL.Helpers;
using CSConsoleRL.Events;

namespace CSConsoleRL.Ai.Interfaces
{
  public interface IAi
  {
    public IGameEvent GetAiResponse(GameStateHelper gameStateHelper);
    public void ConstructAiStateMachine();
  }
}