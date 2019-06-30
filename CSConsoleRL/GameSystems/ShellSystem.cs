using CSConsoleRL.Components;
using CSConsoleRL.Entities;
using CSConsoleRL.Events;
using CSConsoleRL.Game.Managers;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace CSConsoleRL.GameSystems
{
    public class ShellSystem : GameSystem
    {
        struct ShellCommand
        {
            string Cmd;
            string Desc;
            Func<List<string>, string> ShellFunction;
        }

        private Dictionary<string, Func<List<string>, string>> _supportedShellFunctions;

        public ShellSystem(GameSystemManager manager)
        {
            SystemManager = manager;
        }

        public override void InitializeSystem()
        {

        }

        public override void AddEntity(Entity entity)
        {

        }

        public override void HandleMessage(IGameEvent gameEvent)
        {
        
        }

        /// <summary>
        /// Define supported shell functions here for use through the game terminal
        /// </summary>
        private void DefineSupportedShellFunctions()
        {
            _supportedShellFunctions.Add("ce", CreateEntity);
        }

        private string CreateEntity(List<string> inputs)
        {
            return "";
        }


    }
}