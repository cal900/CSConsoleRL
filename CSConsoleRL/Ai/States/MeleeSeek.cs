using CSConsoleRL.Ai.Interfaces;
using CSConsoleRL.Helpers;

namespace CSConsoleRL.Ai.States
{
  delegate bool Condition(GameStateHelper gameStateHelper);
  public class MeleeSeek : IAiState
  {
    public MeleeSeek()
    {

    }

    public string GetName()
    {
      return "MeleeSeek";
    }
    
    public void GetAiStateResponse(GameStateHelper gameStateHelper)
    {

    }
  }
}