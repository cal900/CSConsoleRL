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
        private const Keyboard.Key _inputMenuUp = Keyboard.Key.Up;
        private const Keyboard.Key _inputMenuDown = Keyboard.Key.Down;
        private const Keyboard.Key _inputToggleConsole = Keyboard.Key.Tilde;

        private Keyboard.Key _lastKeyPressed;
        private bool _lastKeyPressedIsDirty; //Used for situations such as console open where don't want to take multiple from a key being held down

        private bool _consoleOn;

        private List<Entity> _userInputEntities;
        private Queue<Keyboard.Key> _inputs;

        public UserInputSystem(GameSystemManager _manager, RenderWindow _sfmlWindow)
        {
            SystemManager = _manager;
            _userInputEntities = new List<Entity>();

            sfmlWindow = _sfmlWindow;
            sfmlWindow.KeyPressed += SfmlWindow_KeyPressed;
            sfmlWindow.KeyReleased += SfmlWindow_KeyReleased;
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
                _userInputEntities.Add(entity);
            }
        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "NextFrame":
                    NextFrame();
                    break;
            }
        }

        private void NextFrame()
        {
            HandleInputs();
        }

        private void HandleInputs()
        {
            while(_inputs.Count > 0)
            {
                HandleInput(_inputs.Dequeue());
                BroadcastMessage(new NextTurnEvent());
            }
        }

        private void HandleInput(Keyboard.Key input)
        {
            switch(input)
            {
                case (_inputUp):
                    BroadCastMovementInputToAllEntities(EnumDirections.North);
                    break;
                case (_inputDown):
                    BroadCastMovementInputToAllEntities(EnumDirections.South);
                    break;
                case (_inputLeft):
                    BroadCastMovementInputToAllEntities(EnumDirections.West);
                    break;
                case (_inputRight):
                    BroadCastMovementInputToAllEntities(EnumDirections.East);
                    break;
                case (_inputToggleConsole):
                    BroadcastMessage(new ToggleConsoleEvent());
                    break;
                case (Keyboard.Key.Escape):
                    BroadcastMessage(new ExitGameEvent());
                    break;
            }
        }

        private void BroadCastMovementInputToAllEntities(EnumDirections dir)
        {
            foreach (Entity entity in _userInputEntities.Where(ent => ent.GetType() == typeof(ActorEntity)))
            {
                BroadcastMessage(new MovementInputEvent(entity.Id, dir));
            }
        }
    }
}
