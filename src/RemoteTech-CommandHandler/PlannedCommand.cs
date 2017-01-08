using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class PlannedCommand
    {
        public const string configNodeName = "PlannedCommand";
        public const string commandNodeName = "Command";
        public const string conditionNodeName = "Condition";

        public readonly ICommand command;
        public readonly ICondition condition;
        public readonly Guid vesselId;

        public PlannedCommand(ConfigNode node)
        {
            ConfigNode subNode = null;
            if (node.TryGetNode(ProviderManager.providerDataNodeName, ref subNode))
            {
                command = LoadCommand(subNode);
            }
        }

        public PlannedCommand(ICommand cmd, ICondition cond, Guid vesselGuid)
        {
            command = cmd;
            condition = cond;
            vesselId = vesselGuid;
        }

        public void Save(ConfigNode node)
        {
            // save command
            var subNode = node.AddNode(commandNodeName);
            subNode.AddValue(ProviderManager.providerConfigLabelName, command.ProviderName);
            subNode = subNode.AddNode(ProviderManager.providerDataNodeName);
            command.Save(subNode);
            var subcmd = command.SubCommands;
            for (var i = 0; i < subcmd.Length; i++)
            {
                subNode = node.AddNode(commandNodeName);
                subcmd[i].Save(subNode);
            }
            // save condition
            
        }

        private ICommand LoadCommand(ConfigNode node)
        {
            string providerName = string.Empty;
            ConfigNode extNode = null;
            if (node.TryGetValue(ProviderManager.providerConfigLabelName, ref providerName) && node.TryGetNode(ProviderManager.providerDataNodeName, ref extNode))
            {
                var subnodes = node.GetNodes(commandNodeName);
                var subcmd = new ICommand[subnodes.Length];
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subcmd[i] = LoadCommand(subnodes[i]);
                }
                var provider = ProviderManager.FindProvider(providerName);
                if (provider != null)
                {
                    var cmd = provider.LoadCommand(extNode);
                    for (var i = 0; i < subcmd.Length; i++)
                    {
                        cmd.AddSubCommand(subcmd[i]);
                    }
                    return cmd;
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
            return null;
        }
    }
}
