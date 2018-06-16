using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.SPACECENTER)]
    class RTCommandHandlerDataScenario : ScenarioModule
    {
        private const string commandManagerNodeName = "CommandManager";

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            return;//temp
            if (node.HasNode(commandManagerNodeName))
            {
                CommandManager.Instance.Load(node.GetNode(commandManagerNodeName));
            }
            else
            {
                // log COmmandManager node not found
                CommandManager.Instance.Load(new ConfigNode());
            }
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
            return;//temp
            CommandManager.Instance.Save(node.AddNode(commandManagerNodeName));
        }
    }
}
