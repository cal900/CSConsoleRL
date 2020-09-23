using System;
using System.Collections.Generic;
using System.Text;
using CSConsoleRL.Entities;
using CSConsoleRL.Components;
using SFML.System;

namespace CSConsoleRL.Helpers
{
  public enum EnumGameState
  {
    MainMenu,
    RegularGame,
    Console,
    Targeting
  }

  public sealed class GameStateHelper
  {
    public EnumGameState GameState { get; private set; }
    public Vector2i CameraCoords { get; private set; }
    public GameStateHelper()
    {
      GameState = EnumGameState.RegularGame;
    }

    public void SetGameState(EnumGameState newState)
    {
      GameState = newState;
    }

    public void SetCameraCoords(int x, int y)
    {
      CameraCoords = new Vector2i(x, y);
    }
  }
}
