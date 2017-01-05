using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.SPACECENTER)]
    class RTCommandHandlerDataScenario : ScenarioModule
    {
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            if (node.HasNode(CommandManager.configNodeName))
            {
                CommandManager.Instance.LoadFromConfigNode(node.GetNode(CommandManager.configNodeName));
            }
            else
            {
                CommandManager.Instance.LoadFromConfigNode(new ConfigNode());
            }
            //if (node.HasNode(CommandPlanner.configNodeName))
            //{
            //    CommandPlanner.Instance.LoadFromConfigNode(node.GetNode(CommandPlanner.configNodeName));
            //}
            //else
            //{
            //    CommandPlanner.Instance.LoadFromConfigNode(new ConfigNode());
            //}
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
        }
    }
}
