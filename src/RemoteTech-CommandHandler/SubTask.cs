using System;
using System.Collections.Generic;

namespace RemoteTech.CommandHandler
{
    public class SubTask
    {
        private const string labelKeyName = "label";
        private const string requiredKeyName = "required";
        private const string taskNodeName = "Task";

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

        public Task Task
        {
            get;
            set;
        }

        public SubTask(string label, bool required)
        {
            Label = label;
            Required = required;
        }

        public SubTask(string label, bool required, Task action)
        {
            Label = label;
            Required = required;
            Task = action;
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(labelKeyName, Label);
            node.AddValue(requiredKeyName, Required);
            Task.Save(node.AddNode(taskNodeName));
        }

        public void Load(ConfigNode node)
        {
            var labelVal = string.Empty;
            var requiredVal = string.Empty;
            ConfigNode taskNode = null;
            if (node.TryGetValue(labelKeyName, ref labelVal) &&
                node.TryGetValue(requiredKeyName, ref requiredVal) &&
                node.TryGetNode(taskNodeName, ref taskNode))
            {
                try
                {
                    var req = bool.Parse(requiredVal);
                    Label = labelVal;
                    Required = req;
                    Task = Task.LoadFrom(taskNode);
                }
                catch (Exception ex)
                {
                    // log ex.ToString();
                    ex.ToString();
                }
            }
            else
            {
                if (taskNode == null)
                {
                    // log "Unable to to find condition node"
                }
            }
        }

        public static SubTask LoadFrom(ConfigNode node)
        {
            var labelVal = string.Empty;
            var requiredVal = string.Empty;
            ConfigNode taskNode = null;
            if (node.TryGetValue(labelKeyName, ref labelVal) &&
                node.TryGetValue(requiredKeyName, ref requiredVal) &&
                node.TryGetNode(taskNodeName, ref taskNode))
            {
                try
                {
                    var req = bool.Parse(requiredVal);
                    return new SubTask(labelVal, req, Task.LoadFrom(taskNode));
                }
                catch (Exception ex)
                {
                    // log ex.ToString();
                    ex.ToString();
                }
            }
            else
            {
                if (taskNode == null)
                {
                    // log "Unable to to find condition node"
                }
            }
            return null;
        }
    }
}
