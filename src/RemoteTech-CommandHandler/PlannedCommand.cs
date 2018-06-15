using System;
using System.Collections.Generic;

namespace RemoteTech.CommandHandler
{
    public class PlannedCommand
    {
        private const string configNodeName = "PlannedCommand";
        private const string taskNodeName = "Task";
        private const string conditionNodeName = "Condition";
        private const string idKeyName = "id";
        private const string enabledKeyName = "enabled";
        private const string oneShotKeyName = "oneShot";

        public Task Task
        {
            get;
            private set;
        }

        public Condition Condition
        {
            get;
            private set;
        }

        public Guid Id
        {
            get;
            private set;
        }

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

        public PlannedCommand(Task task, Condition cond, bool enabled, bool oneShot) :
            this(task, cond, CommandManager.Instance.NewCommandId(), enabled, oneShot)
        { }

        private PlannedCommand(Task task, Condition cond, Guid id, bool enabled, bool oneShot)
        {
            Task = task;
            Condition = cond;
            Id = id;
            Enabled = enabled;
            OneShot = oneShot;
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(idKeyName, Id.ToString());
            node.AddValue(enabledKeyName, Enabled);
            node.AddValue(oneShotKeyName, OneShot);
            Condition.Save(node.AddNode(conditionNodeName));
            Task.Save(node.AddNode(taskNodeName));
        }

        public static PlannedCommand Load(ConfigNode node)
        {
            string idVal = string.Empty;
            string enVal = string.Empty;
            string osVal = string.Empty;
            ConfigNode taskNode = null;
            ConfigNode condNode = null;
            if (node.TryGetValue(idKeyName, ref idVal) &&
                node.TryGetValue(enabledKeyName, ref enVal) &&
                node.TryGetValue(oneShotKeyName, ref osVal) &&
                node.TryGetNode(taskNodeName, ref taskNode) &&
                node.TryGetNode(conditionNodeName, ref condNode))
            {
                try
                {
                    var id = new Guid(idVal);
                    var enabled = bool.Parse(enVal);
                    var oneShot = bool.Parse(osVal);
                    return new PlannedCommand(Task.LoadFrom(taskNode), Condition.LoadFrom(condNode), id, enabled, oneShot);
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
                if (taskNode == null)
                {
                    // log "Unable to to find command node"
                }
            }
            return null;
        }
    }
}
