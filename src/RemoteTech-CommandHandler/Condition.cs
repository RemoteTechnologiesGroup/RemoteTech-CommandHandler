using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class Condition : IConfigNode
    {
        private const string commandNodeName = "Command";

        public ICondition extCondition;

        private Condition[] subConditions;

        //public Command(){}

        public Condition(ConfigNode node)
        {
            Load(node);
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(ProviderManager.providerConfigLabelName, extCondition.ProviderName);
            var extNode = new ConfigNode(ProviderManager.providerDataNodeName);
            extCondition.Save(extNode);
            node.AddNode(extNode);
            for (var i = 0; i < subConditions.Length; i++)
            {
                extNode = new ConfigNode(commandNodeName);
                subConditions[i].Save(extNode);
                node.AddNode(extNode);
            }
        }

        public void Load(ConfigNode node)
        {
            string providerName = string.Empty;
            ConfigNode extNode = null;
            if (node.TryGetValue(ProviderManager.providerConfigLabelName, ref providerName) && node.TryGetNode(ProviderManager.providerDataNodeName, ref extNode))
            {
                var subnodes = node.GetNodes(commandNodeName);
                subConditions = new Condition[subnodes.Length];
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subConditions[i] = new Condition(subnodes[i]);
                }
                var provider = ProviderManager.FindProvider(providerName);
                if (provider != null)
                {
                    extCondition = provider.LoadCondition(extNode);
                    for (var i = 0; i < subConditions.Length; i++)
                    {
                        extCondition.AddSubCondition(subConditions[i].extCondition);
                    }
                }
                else
                {
                    // log "Unable to find provider with name 'providerName'"
                }
            }
            else
            {
                if (providerName == string.Empty)
                {
                    // log "Unable to get provider name from config node"
                }
                if (extNode == null)
                {
                    // log "Unable to get provider data from config node"
                }
            }
        }
    }
}
