using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class Command : IConfigNode
    {
        private const string commandNodeName = "Command";
        private ICommand extCommand;
        private Command[] subCommands;

        //public Command(){}

        public Command(ConfigNode node)
        {
            Load(node);
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(ProviderManager.providerConfigLabelName, extCommand.providerName);
            var extNode = new ConfigNode(ProviderManager.providerDataNodeName);
            extCommand.Save(extNode);
            node.AddNode(extNode);
            for(var i = 0; i < subCommands.Length; i++)
            {
                extNode = new ConfigNode(commandNodeName);
                subCommands[i].Save(extNode);
                node.AddNode(extNode);
            }
        }

        public void Load(ConfigNode node)
        {
            string providerName = string.Empty;
            ConfigNode extNode = null;
            if (node.TryGetValue(ProviderManager.providerConfigLabelName, ref providerName) && node.TryGetNode(ProviderManager.providerDataNodeName, ref extNode))
            {
                var provider = ProviderManager.Instance.FindProvider(providerName);
                if (provider != null)
                {
                    extCommand = provider.LoadCommand(extNode);
                }
                else
                {
                    // log "Unable to find provider with name 'providerName'"
                }
                var subnodes = node.GetNodes(commandNodeName);
                subCommands = new Command[subnodes.Length];
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subCommands[i] = new Command(subnodes[i]);
                }
            }
            else
            {
                if(providerName == string.Empty)
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
