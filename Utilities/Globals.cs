using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using CSConsoleRL.Logging;

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
      var preferencesPath = @"preferences.json";

      using (StreamReader sr = new StreamReader(preferencesPath))
      {
        GameLogger.Instance().LogDebug("Attempting to read preferences from path {@path}",
          Path.Combine(Environment.CurrentDirectory, preferencesPath)
        );
        string json = sr.ReadToEnd();
        _preferences = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
      }
    }

    // Use generics so don't have to cast object as intended type when used
    // ints are parsed as Int64, decimals as double
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