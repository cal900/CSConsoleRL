using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.GameSystems;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Components;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using GameTiles.Tiles;
using GameTiles;
using CSConsoleRL.Logging;
using CSConsoleRL.Data;

namespace CSConsoleRL.Game.Managers
{
  public class GameSystemManager
  {
    public event EventHandler WaitingForInput;
    public event EventHandler DoneWithInput;
    public List<Entity> Entities { get; set; }
    public Dictionary<Type, GameSystem> Systems { get; set; }
    private List<Entity> _entitiesToRemove { get; set; }
    private RenderWindow _sfmlWindow { get; set; }
    private MapFile _gameMap { get; set; }

    private bool exitGame;

    public GameSystemManager(MapFile _gameMap)
    {
      this._gameMap = _gameMap;
      Entities = new List<Entity>();
      Systems = new Dictionary<Type, GameSystem>();

      _sfmlWindow = new RenderWindow(new VideoMode(620, 620), "CSConsoleRL");
      _entitiesToRemove = new List<Entity>();

      GameLogger.Instance();

      CreateSystems();

      CreateEntities();

      InitializeSystems();

      MainGameLoop();
    }

    private void CreateEntities()
    {
      var mainChar = new ActorEntity();
      RegisterEntity(mainChar);
      mainChar.GetComponent<InventoryComponent>().AddItem(new Item(EnumItemTypes.Knife));
      ((LosSystem)Systems[typeof(LosSystem)]).SystemEntities = mainChar;
      var snapCamera = new SnapCameraToEntityEvent(mainChar);
      BroadcastEvent(snapCamera);
    }

    private void CreateSystems()
    {
      RegisterSystem(new MovementSystem(this, _gameMap.TileSet));
      RegisterSystem(new LosSystem(this, _gameMap.TileSet));
      RegisterSystem(new SfmlGraphicsSystem(this, _sfmlWindow, _gameMap.TileSet));
      RegisterSystem(new UserInputSystem(this, _sfmlWindow));
      RegisterSystem(new TerminalSystem(this));
      RegisterSystem(new AiSystem(this, _gameMap.TileSet));
      RegisterSystem(new InventorySystem(this));
    }

    private void InitializeSystems()
    {
      foreach (KeyValuePair<Type, GameSystem> system in Systems)
      {
        system.Value.InitializeSystem();
      }
    }

    private GameSystem RegisterSystem(GameSystem system)
    {
      Systems.Add(system.GetType(), system);
      system.SystemManager = this;
      return system;
    }

    public Entity RegisterEntity(Entity entity)
    {
      Entities.Add(entity);

      foreach (KeyValuePair<Type, GameSystem> system in Systems)
      {
        system.Value.AddEntity(entity);
      }

      return entity;
    }

    public List<Entity> GetEntities<T>()
    {
      var matchingEnts = new List<Entity>();
      foreach (var ent in Entities)
      {
        if (ent.GetType() == typeof(T))
        {
          matchingEnts.Add(ent);
        }
      }

      return matchingEnts;
    }

    public List<Entity> GetEntities()
    {
      var matchingEnts = new List<Entity>();
      foreach (var ent in Entities)
      {
        matchingEnts.Add(ent);
      }

      return matchingEnts;
    }

    /// <summary>
    /// Entities are only removed at beginning of frame, this marks as to be deleted
    /// </summary>
    /// <param name="entity"></param>
    public void RemoveEntity(Entity entity)
    {
      _entitiesToRemove.Add(entity);
    }

    /// <summary>
    /// Removes all entities that have been marked as to be deleted
    /// </summary>
    /// <param name="entity"></param>
    private void DeleteMarkedEntities()
    {
      for (int i = _entitiesToRemove.Count - 1; i >= 0; i--)
      {
        foreach (var system in Systems.Values)
        {
          system.RemoveEntity(_entitiesToRemove[i]);
        }

        Entities.Remove(_entitiesToRemove[i]);
      }
    }

    public void BroadcastUiEvent(/*GameEvent evt*/)
    {
      //if(_gameStateManager != null)_gameStateManager.CurrentState().HandleEvent(evt);
    }

    public void BroadcastEvent(IGameEvent evnt)
    {
      if (evnt.EventName == "ExitGame")
      {
        exitGame = true;
      }

      foreach (KeyValuePair<Type, GameSystem> system in Systems)
      {
        system.Value.HandleMessage(evnt);
      }
    }

    public Entity WithId(Guid id)
    {
      return Entities.SingleOrDefault(e => e.Id == id);
    }

    private void MainGameLoop()
    {
      //Clock clock = new Clock();
      _sfmlWindow.SetFramerateLimit(30);
      _sfmlWindow.SetVerticalSyncEnabled(false);

      var nextFrameEvent = new NextFrameEvent();

      while (_sfmlWindow.IsOpen && !exitGame)
      {
        //var timeElapsed = clock.ElapsedTime;
        DeleteMarkedEntities();
        _sfmlWindow.DispatchEvents();
        BroadcastEvent(nextFrameEvent);
      }
    }
  }
}