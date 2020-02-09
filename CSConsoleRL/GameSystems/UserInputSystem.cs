using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSConsoleRL.Game.Managers;
using CSConsoleRL.Components;
using CSConsoleRL.Components.Interfaces;
using CSConsoleRL.Events;
using CSConsoleRL.Enums;
using CSConsoleRL.Entities;
using SFML.Graphics;
using SFML.Window;


//Note - it is the responsibility of UserInputSystem to know what command keyboard inputs correspond to depending on the Component type.
//Otherwise will get too complicated
namespace CSConsoleRL.GameSystems
{
  public class UserInputSystem : GameSystem
  {
    private RenderWindow sfmlWindow;

    private const Keyboard.Key _inputUp = Keyboard.Key.W;
    private const Keyboard.Key _inputDown = Keyboard.Key.S;
    private const Keyboard.Key _inputLeft = Keyboard.Key.A;
    private const Keyboard.Key _inputRight = Keyboard.Key.D;
    private const Keyboard.Key _inputUpLeft = Keyboard.Key.Q;
    private const Keyboard.Key _inputUpRight = Keyboard.Key.E;
    private const Keyboard.Key _inputDownLeft = Keyboard.Key.Z;
    private const Keyboard.Key _inputDownRight = Keyboard.Key.X;
    private const Keyboard.Key _inputMenuUp = Keyboard.Key.Up;
    private const Keyboard.Key _inputMenuDown = Keyboard.Key.Down;
    private const Keyboard.Key _inputToggleConsole = Keyboard.Key.Tilde;
    private const Keyboard.Key _inputToggleActiveItem = Keyboard.Key.Tab;

    private Keyboard.Key _lastKeyPressed;
    private bool _lastKeyPressedIsDirty; //Used for situations such as console open where don't want to take multiple from a key being held down

    private bool _consoleOn;
    private Queue<Keyboard.Key> _inputs;

    public UserInputSystem(GameSystemManager _manager, RenderWindow _sfmlWindow)
    {
      SystemManager = _manager;
      _systemEntities = new List<Entity>();

      sfmlWindow = _sfmlWindow;
      sfmlWindow.KeyPressed += SfmlWindow_KeyPressed;
      sfmlWindow.KeyReleased += SfmlWindow_KeyReleased;

      _inputs = new Queue<Keyboard.Key>();
    }

    private void SfmlWindow_KeyPressed(object sender, KeyEventArgs e)
    {
      _lastKeyPressed = e.Code;
      _inputs.Enqueue(e.Code);
    }

    private void SfmlWindow_KeyReleased(object sender, KeyEventArgs e)
    {
      if (e.Code == _lastKeyPressed)
      {
        _lastKeyPressed = Keyboard.Key.Unknown;
      }
    }

    public override void InitializeSystem()
    {

    }

    public override void AddEntity(Entity entity)
    {
      if (entity.Components.ContainsKey(typeof(UserInputComponent)))
      {
        _systemEntities.Add(entity);
      }
    }

    public override void HandleMessage(IGameEvent gameEvent)
    {
      switch (gameEvent.EventName)
      {
        case "NextFrame":
          NextFrame();
          break;
        case "ToggleConsole":
          _consoleOn = !_consoleOn;
          break;
      }
    }

    private void NextFrame()
    {
      HandleInputs();
    }

    private void HandleInputs()
    {
      while (_inputs.Count > 0)
      {
        if (HandleInput(_inputs.Dequeue()))
        {
          BroadcastMessage(new NextTurnEvent());
        }
      }
    }

    /// <summary>
    /// Returns bool indicating if input corresponds to a turn (e.g. movement)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private bool HandleInput(Keyboard.Key input)
    {
      if (!_consoleOn)
      {
        switch (input)
        {
          case (_inputUp):
            BroadCastMovementInputToAllEntities(0, -1);
            return true;
          case (_inputDown):
            BroadCastMovementInputToAllEntities(0, 1);
            return true;
          case (_inputLeft):
            BroadCastMovementInputToAllEntities(-1, 0);
            return true;
          case (_inputRight):
            BroadCastMovementInputToAllEntities(1, 0);
            return true;
          case (_inputUpLeft):
            BroadCastMovementInputToAllEntities(-1, -1);
            return true;
          case (_inputUpRight):
            BroadCastMovementInputToAllEntities(1, -1);
            return true;
          case (_inputDownLeft):
            BroadCastMovementInputToAllEntities(-1, 1);
            return true;
          case (_inputDownRight):
            BroadCastMovementInputToAllEntities(1, 1);
            return true;
          case (_inputToggleConsole):
            BroadcastMessage(new ToggleConsoleEvent());
            break;
          case (_inputToggleActiveItem):
            b
                    case (Keyboard.Key.Escape):
            BroadcastMessage(new ExitGameEvent());
            break;
        }
      }
      else
      {
        BroadcastMessage(new KeyPressedEvent(input));
      }

      return false;
    }

    private void BroadCastMovementInputToAllEntities(int changeX, int changeY)
    {
      foreach (Entity entity in _systemEntities.Where(ent => ent.GetType() == typeof(ActorEntity)))
      {
        BroadcastMessage(new MovementInputEvent(entity.Id, changeX, changeY));
      }
    }
  }
}
