using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech.CommandHandler
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class CommandManager : MonoBehaviour
    {
        public const string configNodeName = "CommandManager";

        private const int initialListSize = 50;

        private static CommandManager instance;

        public static CommandManager Instance
        {
            get
            {
                return instance;
            }
        }

        private List<PlannedCommand> plannedCommands = new List<PlannedCommand>(initialListSize);
        private List<PlannedCommand> delayedCommands = new List<PlannedCommand>(initialListSize);
        private bool needToLoad = true;

        public void Awake()
        {
            GameEvents.onGameStateLoad.Add(new EventData<ConfigNode>.OnEvent(GameLoadEvent));
        }

        public void OnDestroy()
        {
            GameEvents.onGameStateLoad.Remove(new EventData<ConfigNode>.OnEvent(GameLoadEvent));
        }

        /// <summary>
        /// Adds command to the CommandPlanner if capture is on, otherwise queues the command for active vessel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool AddCommand(ICommand command)
        {
            return true;
        }

        public bool AddPlannedCommand(ICommand command, ICondition condition)
        {
            return true;
        }

        public void LoadFromConfigNode(ConfigNode node)
        {
            if ( !needToLoad || !node.HasNode(PlannedCommand.configNodeName))
            {
                return;
            }
            plannedCommands.Clear();
            var nodes = node.GetNodes(PlannedCommand.configNodeName);
            for (var i=0; i < nodes.Length; i++)
            {
                plannedCommands.Add(new PlannedCommand(nodes[i]));
            }
            needToLoad = false;
        }

        private void GameLoadEvent(ConfigNode node)
        {
            needToLoad = true;
        }

        public void FixedUpdate()
        {
        }
    }
}
