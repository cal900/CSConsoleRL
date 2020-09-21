using System;
using System.Collections.Generic;
using System.Text;
using CSConsoleRL.Entities;
using CSConsoleRL.Components;

namespace CSConsoleRL.Helpers
{
  public enum EnumGameState
  {
    MainMenu,
    RegularGame,
    Aiming
  }

  public sealed class GameStateHelper
  {
    public EnumGameState GameState { get; private set; }
    public GameStateHelper()
    {
      GameState = EnumGameState.RegularGame;
    }

    public void SetGameState(EnumGameState newState)
    {
      GameState = newState;
    }
  }
}
