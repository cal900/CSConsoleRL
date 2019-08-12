using System;
using System.Collections.Generic;

namespace Utilities
{
  public sealed class GameGlobals
  {
    private static GameGlobals _instance;
    private Dictionary<string, object> _preferences;

    private GameGlobals()
    {
      ReadPreferences();
    }

    public static GameGlobals Instance()
    {
      if (_instance == null)
      {
        _instance = new GameGlobals();
      }

      return _instance;
    }

    private void ReadPreferences()
    {

    }

    // Use generics so don't have to cast object as intended type when used
    public T Get<T>(string key)
    {
      if (_preferences.ContainsKey(key))
      {
        return (T)_preferences[key];
      }
      else
      {
        throw new Exception("Key {key} not found in preferences");
      }
    }
  }
}