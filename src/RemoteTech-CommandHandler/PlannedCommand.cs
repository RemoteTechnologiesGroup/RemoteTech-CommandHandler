using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    public class PlannedCommand
    {
        public const string configNodeName = "PlannedCommand";
        public const string actionNodeName = "Action";
        public const string conditionNodeName = "Condition";
        public const string idKeyName = "id";
        public const string enabledKeyName = "enabled";
        public const string oneShotKeyName = "oneShot";

        public readonly IAction action;
        public readonly ICondition condition;
        public readonly Guid id;

        public bool Enabled
        {
            get;
            set;
        }

        public bool OneShot
        {
            get;
            set;
        }

        public PlannedCommand(ConfigNode node)
        {
            string idVal = string.Empty;
            string enVal = string.Empty;
            string osVal = string.Empty;
            ConfigNode cmdNode = null;
            ConfigNode condNode = null;
            if (node.TryGetValue(idKeyName, ref idVal) &&
                node.TryGetValue(enabledKeyName, ref enVal) &&
                node.TryGetValue(oneShotKeyName, ref osVal) &&
                node.TryGetNode(actionNodeName, ref cmdNode) &&
                node.TryGetNode(conditionNodeName, ref condNode))
            {
                try
                {
                    id = new Guid(idVal);
                    Enabled = bool.Parse(enVal);
                    OneShot = bool.Parse(osVal);
                }
                catch (Exception ex)
                {
                    // log ex.ToString();
                }
                action = LoadAction(cmdNode);
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

        public PlannedCommand(IAction act, ICondition cond, bool enabled, bool oneShot)
        {
            action = act;
            condition = cond;
            id = CommandManager.Instance.NewCommandId();
            Enabled = enabled;
            OneShot = oneShot;
        }

        //public void Enable()
        //{
        //    Enabled = true;
        //}

        //public void Disable()
        //{
        //    Enabled = false;
        //}

        //public void ToggleEnabled()
        //{
        //    Enabled = !Enabled;
        //}

        public void Save(ConfigNode node)
        {
            // save values
            node.AddValue(idKeyName, id.ToString());
            node.AddValue(enabledKeyName, Enabled);
            node.AddValue(oneShotKeyName, OneShot);
            // save command
            var subNode = node.AddNode(actionNodeName);
            subNode.AddValue(ProviderManager.providerConfigLabelName, action.ProviderName);
            subNode = subNode.AddNode(ProviderManager.providerDataNodeName);
            action.Save(subNode);
            var subCmd = action.SubCommands;
            for (var i = 0; i < subCmd.Length; i++)
            {
                subNode = node.AddNode(actionNodeName);
                subCmd[i].Save(subNode);
            }
            // save condition
            subNode = node.AddNode(conditionNodeName);
            subNode.AddValue(ProviderManager.providerConfigLabelName, action.ProviderName);
            subNode = subNode.AddNode(ProviderManager.providerDataNodeName);
            condition.Save(subNode);
            var subCond = condition.SubConditions;
            for (var i = 0; i < subCond.Length; i++)
            {
                subNode = node.AddNode(actionNodeName);
                subCond[i].Save(subNode);
            }
        }

        private IAction LoadAction(ConfigNode node)
        {
            string providerName = string.Empty;
            ConfigNode extNode = null;
            if (node.TryGetValue(ProviderManager.providerConfigLabelName, ref providerName) && node.TryGetNode(ProviderManager.providerDataNodeName, ref extNode))
            {
                var subnodes = node.GetNodes(actionNodeName);
                var subact = new IAction[subnodes.Length];
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subact[i] = LoadAction(subnodes[i]);
                }
                var provider = ProviderManager.FindProvider(providerName);
                if (provider != null)
                {
                    var act = provider.LoadAction(extNode);
                    for (var i = 0; i < subact.Length; i++)
                    {
                        act.AddSubAction(subact[i]);
                    }
                    return act;
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
                var subnodes = node.GetNodes(actionNodeName);
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
