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

namespace CSConsoleRL.GameSystems
{
    public class UserInputSystem : GameSystem
    {
        private const ConsoleKey InputUp = ConsoleKey.UpArrow;
        private const ConsoleKey InputDown = ConsoleKey.DownArrow;
        private const ConsoleKey InputLeft = ConsoleKey.LeftArrow;
        private const ConsoleKey InputRight = ConsoleKey.RightArrow;

        private Keyboard.Key lastKeyPressed;
        private bool lastKeyPressedIsDirty;

        private List<Entity> userInputEntities;

        public UserInputSystem(GameSystemManager manager, RenderWindow sfmlWindow)
        {
            SystemManager = manager;
            userInputEntities = new List<Entity>();

            sfmlWindow.KeyPressed += sfmlWindow_KeyPressed;
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

        private void NextFrame()
        {
            HandleKeyPressed();
        }

        private void HandleKeyPressed()
        {
            if (!lastKeyPressedIsDirty)
            {
                foreach(Entity entity in userInputEntities)
                {
                    var 
                }

                //need broadcast movement event to some sort of movementSystem
                //will need ID of entity/component trying to move
                //movementSystem can then handle collision/w.e.

                lastKeyPressedIsDirty = true;
            }
        }
    }
}
