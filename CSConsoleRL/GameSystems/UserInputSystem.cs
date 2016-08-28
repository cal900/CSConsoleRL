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

        private bool exitGame;

        public UserInputSystem(GameSystemManager manager, RenderWindow sfmlWindow, ref bool _exitGame)
        {
            SystemManager = manager;
            userInputEntities = new List<Entity>();

            sfmlWindow.KeyPressed += sfmlWindow_KeyPressed;

            exitGame = _exitGame;
        }

        public override void InitializeSystem()
        {

        }

        void sfmlWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            //lastKeyPressed = e.Code;
            //lastKeyPressedIsDirty = false;
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

        public override void BroadcastMessage(IGameEvent evnt)
        {
            SystemManager.BroadcastEvent(evnt);
        }

        private void NextFrame()
        {
            HandleKeyPressed();
        }

        private void HandleKeyPressed()
        {
            //if (!lastKeyPressedIsDirty)
            //{
            //    foreach (Entity entity in userInputEntities)
            //    {
            //        if (entity.GetType() == typeof(ActorEntity))
            //        {
            //            if (lastKeyPressed == InputUp)
            //            {
            //                var movementEvent = new MovementEvent(entity.Id, EnumDirections.North);
            //                BroadcastMessage(movementEvent);
            //            }
            //            else if (lastKeyPressed == InputDown)
            //            {
            //                var movementEvent = new MovementEvent(entity.Id, EnumDirections.South);
            //                BroadcastMessage(movementEvent);
            //            }
            //            else if (lastKeyPressed == InputLeft)
            //            {
            //                var movementEvent = new MovementEvent(entity.Id, EnumDirections.West);
            //                BroadcastMessage(movementEvent);
            //            }
            //            else if (lastKeyPressed == InputRight)
            //            {
            //                var movementEvent = new MovementEvent(entity.Id, EnumDirections.East);
            //                BroadcastMessage(movementEvent);
            //            }
            //            else if(lastKeyPressed == Keyboard.Key.Escape)
            //            {
            //                exitGame = true;
            //            }
            //        }
            //    }

            //    //need broadcast movement event to some sort of movementSystem
            //    //will need ID of entity/component trying to move
            //    //movementSystem can then handle collision/w.e.

            //    lastKeyPressedIsDirty = true;
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
                    else if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                    {
                        exitGame = true;
                    }
                }
            }
        }
    }
}
