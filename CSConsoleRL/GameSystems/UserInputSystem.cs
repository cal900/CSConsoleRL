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
        private const Keyboard.Key InputUp = Keyboard.Key.Up;
        private const Keyboard.Key InputDown = Keyboard.Key.Down;
        private const Keyboard.Key InputLeft = Keyboard.Key.Left;
        private const Keyboard.Key InputRight = Keyboard.Key.Right;

        private Keyboard.Key lastKeyPressed;
        private bool lastKeyPressedIsDirty;

        private List<Entity> userInputEntities;

        public UserInputSystem(GameSystemManager manager, RenderWindow sfmlWindow)
        {
            SystemManager = manager;
            userInputEntities = new List<Entity>();

            sfmlWindow.KeyPressed += sfmlWindow_KeyPressed;
        }

        public override void InitializeSystem()
        {
            throw new NotImplementedException();
        }

        void sfmlWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            lastKeyPressed = e.Code;
            lastKeyPressedIsDirty = false;
        }

        public override void AddEntity(Entity entity)
        {
            if (entity.Components.ContainsKey(typeof(UserInputComponent)))
            {
                userInputEntities.Add(entity);
            }
        }

        public override void HandleMessage(GameEvent gameEvent)
        {
            switch (gameEvent.EventName)
            {
                case "NextFrame":
                    NextFrame();
                    break;
            }
        }

        public override GameEvent BroadcastMessage(GameEvent evnt)
        {
            throw new NotImplementedException();
        }

        private void NextFrame()
        {
            HandleKeyPressed();
        }

        private void HandleKeyPressed()
        {
            if (!lastKeyPressedIsDirty)
            {
                foreach (Entity entity in userInputEntities)
                {
                    if (entity.GetType() == typeof(ActorEntity))
                    {
                        if (lastKeyPressed == InputUp)
                        {
                            var movementEvent = new MovementEvent(entity.Id, EnumDirections.North);
                            BroadcastMessage(movementEvent);
                        }
                        else if (lastKeyPressed == InputDown)
                        {
                            var movementEvent = new MovementEvent(entity.Id, EnumDirections.South);
                            BroadcastMessage(movementEvent);
                        }
                        else if (lastKeyPressed == InputLeft)
                        {
                            var movementEvent = new MovementEvent(entity.Id, EnumDirections.West);
                            BroadcastMessage(movementEvent);
                        }
                        else if (lastKeyPressed == InputRight)
                        {
                            var movementEvent = new MovementEvent(entity.Id, EnumDirections.East);
                            BroadcastMessage(movementEvent);
                        }
                    }
                }

                //need broadcast movement event to some sort of movementSystem
                //will need ID of entity/component trying to move
                //movementSystem can then handle collision/w.e.

                lastKeyPressedIsDirty = true;
            }
        }
    }
}
