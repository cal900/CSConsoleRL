﻿using System;
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

namespace CSConsoleRL.GameSystems
{
    public class UserInputSystem : GameSystem
    {
        private const ConsoleKey InputUp = ConsoleKey.UpArrow;
        private const ConsoleKey InputDown = ConsoleKey.DownArrow;
        private const ConsoleKey InputLeft = ConsoleKey.LeftArrow;
        private const ConsoleKey InputRight = ConsoleKey.RightArrow;

        private List<UserInputComponent> userControlComponents;

        public UserInputSystem(GameSystemManager manager)
        {
            SystemManager = manager;
            userControlComponents = new List<UserInputComponent>();
        }

        public override void AddComponent(IComponent component)
        {
            userControlComponents.Add((component as UserInputComponent));
        }

        public override void HandleMessage(GameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public void HandleKeyPressed(ConsoleKeyInfo keyPressed)
        {
            EnumDirections direction = EnumDirections.North;

            if(keyPressed.Key == ConsoleKey.UpArrow)
            {
                direction = EnumDirections.North;
            }
            else if (keyPressed.Key == ConsoleKey.DownArrow)
            {
                direction = EnumDirections.South;
            }
            else if (keyPressed.Key == ConsoleKey.LeftArrow)
            {
                direction = EnumDirections.West;
            }
            else if (keyPressed.Key == ConsoleKey.RightArrow)
            {
                direction = EnumDirections.East;
            }

            var inputEvent = new MovementInputEvent(direction);
            List<Entity> entitiesWithInput = new List<Entity>();

            foreach(UserInputComponent currentComponent in userControlComponents)
            {
                entitiesWithInput.Add(currentComponent.EntityAttachedTo);
            }

            SystemManager.BroadcastEvent(inputEvent, entitiesWithInput);
        }
    }
}
