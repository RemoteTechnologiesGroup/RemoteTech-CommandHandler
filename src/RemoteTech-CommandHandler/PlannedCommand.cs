using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    public class PlannedCommand
    {
        public const string configNodeName = "PlannedCommand";
        public const string commandNodeName = "Command";
        public const string conditionNodeName = "Condition";
        public const string idKeyName = "id";
        public const string enabledKeyName = "enabled";

        public readonly ICommand command;
        public readonly ICondition condition;
        public readonly Guid id;

        public bool Enabled
        {
            get;
            private set;
        }

        public PlannedCommand(ConfigNode node)
        {
            string idVal = string.Empty;
            string enVal = string.Empty;
            ConfigNode cmdNode = null;
            ConfigNode condNode = null;
            if (node.TryGetValue(idKeyName, ref idVal) &&
                node.TryGetValue(enabledKeyName, ref enVal) &&
                node.TryGetNode(commandNodeName, ref cmdNode) &&
                node.TryGetNode(conditionNodeName, ref condNode))
            {
                try
                {
                    id = new Guid(idVal);
                    Enabled = bool.Parse(enVal);
                }
                catch (Exception ex)
                {
                    // log ex.ToString();
                }
                command = LoadCommand(cmdNode);
                condition = LoadCondition(condNode);
            }
            else
            {
                if (cmdNode == null)
                {
                    // log "Unable to to find command node in config node"
                }
                if (condNode == null)
                {
                    // log "Unable to to find condition node in config node"
                }
            }
        }

        public PlannedCommand(ICommand cmd, ICondition cond, bool enabled)
        {
            command = cmd;
            condition = cond;
            id = CommandManager.Instance.NewCommandId();
            Enabled = enabled;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void ToggleEnabled()
        {
            Enabled = !Enabled;
        }

        public void Save(ConfigNode node)
        {
            // save command
            var subNode = node.AddNode(commandNodeName);
            subNode.AddValue(ProviderManager.providerConfigLabelName, command.ProviderName);
            subNode = subNode.AddNode(ProviderManager.providerDataNodeName);
            command.Save(subNode);
            var subCmd = command.SubCommands;
            for (var i = 0; i < subCmd.Length; i++)
            {
                subNode = node.AddNode(commandNodeName);
                subCmd[i].Save(subNode);
            }
            // save condition
            subNode = node.AddNode(conditionNodeName);
            subNode.AddValue(ProviderManager.providerConfigLabelName, command.ProviderName);
            subNode = subNode.AddNode(ProviderManager.providerDataNodeName);
            condition.Save(subNode);
            var subCond = condition.SubConditions;
            for (var i = 0; i < subCond.Length; i++)
            {
                subNode = node.AddNode(commandNodeName);
                subCond[i].Save(subNode);
            }
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

        private ICondition LoadCondition(ConfigNode node)
        {
            string providerName = string.Empty;
            ConfigNode extNode = null;
            if (node.TryGetValue(ProviderManager.providerConfigLabelName, ref providerName) && node.TryGetNode(ProviderManager.providerDataNodeName, ref extNode))
            {
                var subnodes = node.GetNodes(commandNodeName);
                var subCond = new ICondition[subnodes.Length];
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subCond[i] = LoadCondition(subnodes[i]);
                }
                var provider = ProviderManager.FindProvider(providerName);
                if (provider != null)
                {
                    var cond = provider.LoadCondition(extNode);
                    for (var i = 0; i < subCond.Length; i++)
                    {
                        cond.AddSubCondition(subCond[i]);
                    }
                    return cond;
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
