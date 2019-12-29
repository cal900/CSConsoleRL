using System;
using System.Collections.Generic;
using System.IO;
using Serilog;

namespace CSConsoleRL.Logging
{
  /// <summary>
  /// GameLogger should be logging to both the Console and in-game terminal
  /// Want ability to write to output to files as well
  /// </summary>
  public sealed class GameLogger
  {
    private static GameLogger _instance;

    private GameLogger()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();

      Log.Information("Logger initialized");
      Log.Information("Information");
      Log.Debug("Debug");
      Log.Error("Error");
    }

    public static GameLogger Instance()
    {
      if (_instance == null)
      {
        _instance = new GameLogger();
      }

      return _instance;
    }

    public void Cleanup()
    {
      Log.CloseAndFlush();
    }

    public void LogInformation(string info, params object[] parameters)
    {
      Log.Information(info, parameters);
    }

    public void LogDebug(string info, params object[] parameters)
    {
      Log.Debug(info, parameters);
    }

    public void LogError(string info, params object[] parameters)
    {
      Log.Error(info, parameters);
    }
  }
}