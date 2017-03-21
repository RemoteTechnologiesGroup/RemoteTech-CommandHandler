using System;
using System.Collections.Generic;

namespace RemoteTech.CommandHandler
{
    public class SubCondition
    {
        private const string labelKeyName = "label";
        private const string requiredKeyName = "required";
        private const string conditionNodeName = "Condition";

        public string Label
        {
            get;
            private set;
        }

        public bool Required
        {
            get;
            private set;
        }

        public Condition Condition
        {
            get;
            set;
        }

        public SubCondition(string label, bool required)
        {
            Label = label;
            Required = required;
        }

        public SubCondition(string label, bool required, Condition condition)
        {
            Label = label;
            Required = required;
            Condition = condition;
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(labelKeyName, Label);
            node.AddValue(requiredKeyName, Required);
            Condition.Save(node.AddNode(conditionNodeName));
        }

        public void Load(ConfigNode node)
        {
            var labelVal = string.Empty;
            var requiredVal = string.Empty;
            ConfigNode condNode = null;
            if (node.TryGetValue(labelKeyName, ref labelVal) &&
                node.TryGetValue(requiredKeyName, ref requiredVal) &&
                node.TryGetNode(conditionNodeName, ref condNode))
            {
                try
                {
                    var req = bool.Parse(requiredVal);
                    Label = labelVal;
                    Required = req;
                    Condition = Condition.LoadFrom(condNode);
                }
                catch (Exception ex)
                {
                    // log ex.ToString();
                    ex.ToString();
                }
            }
            else
            {
                if (condNode == null)
                {
                    // log "Unable to to find condition node"
                }
            }
        }

        public static SubCondition LoadFrom(ConfigNode node)
        {
            var labelVal = string.Empty;
            var requiredVal = string.Empty;
            ConfigNode condNode = null;
            if (node.TryGetValue(labelKeyName, ref labelVal) &&
                node.TryGetValue(requiredKeyName, ref requiredVal) &&
                node.TryGetNode(conditionNodeName, ref condNode))
            {
                try
                {
                    var req = bool.Parse(requiredVal);
                    return new SubCondition(labelVal, req, Condition.LoadFrom(condNode));
                }
                catch (Exception ex)
                {
                    // log ex.ToString();
                    ex.ToString();
                }
            }
            else
            {
                if (condNode == null)
                {
                    // log "Unable to to find condition node"
                }
            }
            return null;
        }
    }
}
