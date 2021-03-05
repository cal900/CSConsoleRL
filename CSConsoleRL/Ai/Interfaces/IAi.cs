using CSConsoleRL.Helpers;

namespace CSConsoleRL.Ai.Interfaces
{
  public interface IAi
  {
    public void GetAiResponse(GameStateHelper gameStateHelper);
    public void ConstructAiStateMachine();
  }
}