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

        private const Keyboard.Key InputUp = Keyboard.Key.W;
        private const Keyboard.Key InputDown = Keyboard.Key.S;
        private const Keyboard.Key InputLeft = Keyboard.Key.A;
        private const Keyboard.Key InputRight = Keyboard.Key.D;

        private const Keyboard.Key InputMenuUp = Keyboard.Key.Up;
        private const Keyboard.Key InputMenuDown = Keyboard.Key.Down;

        private const Keyboard.Key InputToggleConsole = Keyboard.Key.Tilde;

        private Keyboard.Key lastKeyPressed;
        private bool lastKeyPressedIsDirty; //Used for situations such as console open where don't want to take multiple from a key being held down

        private bool consoleOn;

        private List<Entity> userInputEntities;

        public UserInputSystem(GameSystemManager _manager, RenderWindow _sfmlWindow)
        {
            SystemManager = _manager;
            userInputEntities = new List<Entity>();

            sfmlWindow = _sfmlWindow;
            sfmlWindow.KeyPressed += SfmlWindow_KeyPressed;
            sfmlWindow.KeyReleased += SfmlWindow_KeyReleased;
        }

        private void SfmlWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            lastKeyPressed = e.Code;
        }

        private void SfmlWindow_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == lastKeyPressed)
            {
                lastKeyPressed = Keyboard.Key.Unknown;
            }
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(UserInputComponent)))
            {
                userInputEntities.Add(entity);
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
            HandleKeyPressed();
        }

        private void HandleKeyPressed()
        {
            if (!consoleOn)
            {
                foreach (Entity entity in userInputEntities)
                {
                    if (entity.GetType() == typeof(ActorEntity))
                    {
                        if (Keyboard.IsKeyPressed(InputUp))
                        {
                            var movementEvent = new MovementInputEvent(entity.Id, EnumDirections.North);
                            BroadcastMessage(movementEvent);
                        }
                        else if (Keyboard.IsKeyPressed(InputDown))
                        {
                            var movementEvent = new MovementInputEvent(entity.Id, EnumDirections.South);
                            BroadcastMessage(movementEvent);
                        }
                        else if (Keyboard.IsKeyPressed(InputLeft))
                        {
                            var movementEvent = new MovementInputEvent(entity.Id, EnumDirections.West);
                            BroadcastMessage(movementEvent);
                        }
                        else if (Keyboard.IsKeyPressed(InputRight))
                        {
                            var movementEvent = new MovementInputEvent(entity.Id, EnumDirections.East);
                            BroadcastMessage(movementEvent);
                        }
                        else if (Keyboard.IsKeyPressed(InputToggleConsole))
                        {
                            var toggleConsoleEvent = new ToggleConsoleEvent();
                            BroadcastMessage(toggleConsoleEvent);
                        }
                        else if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                        {
                            var exitEvent = new ExitGameEvent();
                            BroadcastMessage(exitEvent);
                        }
                        else
                        {
                            return;
                        }

                        var nextTurnEvent = new NextTurnEvent();
                        BroadcastMessage(nextTurnEvent);
                    }
                }
            }
            else
            {
                lastKeyPressed.ToString();
            }
        }
    }
}
