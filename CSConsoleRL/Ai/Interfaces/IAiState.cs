using CSConsoleRL.Helpers;
using CSConsoleRL.Events;

namespace CSConsoleRL.Ai.Interfaces
{
  public interface IAiState
  {
    public string GetName();
    public IGameEvent GetAiStateResponse(GameStateHelper gameStateHelper);
  }
}