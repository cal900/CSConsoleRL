using CSConsoleRL.Helpers;

namespace CSConsoleRL.Ai.Interfaces
{
  public interface IAiState
  {
    public string GetName();
    public void GetAiStateResponse(GameStateHelper gameStateHelper);
  }
}