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
    private readonly Dictionary<string, object> _gameState;
    public EnumGameState GameState { get; private set; }
    public Vector2i CameraCoords { get; private set; }
    public GameStateHelper()
    {
      _gameState = new Dictionary<string, object>();
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

    public T GetVar<T>(string key)
    {
      if (!_gameState.ContainsKey(key))
      {
        throw new Exception(String.Format(@"GameStateHelper GetVar was called for variable '{0}'
        that doesn't exist", key));
      }

      var varValue = _gameState[key];

      if (varValue is T)
      {
        return (T)varValue;
      }
      else
      {
        throw new Exception(String.Format(@"GameStateHelper GetVar was called for variable '{0}'
        that exists but is a different type than specified", key));
      }
    }

    public void SetVar(string key, object val)
    {
      if (_gameState.ContainsKey(key))
      {
        var temp = _gameState[key];

        if (temp.GetType() == val.GetType())
        {
          _gameState[key] = val;
        }
        else
        {
          throw new Exception(String.Format(@"GameStateHelper SetVar was called for variable '{0}'
          type '{1}', but the variable already has a value with type '{2}'", key, val.GetType(), temp.GetType()));
        }
      }
      else
      {
        _gameState.Add(key, val);
      }
    }
  }
}
