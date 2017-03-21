using System;
using System.Collections.Generic;

namespace RemoteTech.CommandHandler
{
    public abstract class Task
    {
        private const string nameLabel = "name";
        private const string subTaskNodeName = "SubTask";

        public enum TaskState
        {
            None,
            Ready,
            Running,
            Finished,
            Failed
        }

        public Part part;

        public virtual TaskState State { get; protected set; }
        public virtual List<SubTask> SubTasks { get; protected set; }
        public virtual string DisplayName { get; protected set; }

        public abstract void Activate();
        public abstract void Configure();

        public virtual void OnSave(ConfigNode node) {}

        public virtual void OnLoad(ConfigNode node) {}

        protected Task()
        {
            SubTasks = new List<SubTask>();
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(nameLabel, GetType().Name);
            OnSave(node);
            var subTasks = SubTasks;
            for (var i = 0; i < subTasks.Count; i++)
            {
                subTasks[i].Save(node.AddNode(subTaskNodeName));
            }
        }

        public void Load(ConfigNode node)
        {
            string typeName = string.Empty;
            if (node.TryGetValue(nameLabel, ref typeName))
            {
                if (GetType().Name != typeName)
                {
                    // log an error
                    return;
                }
                var subnodes = node.GetNodes(subTaskNodeName);
                var subTsks = new List<SubTask>(subnodes.Length);
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subTsks.Add(SubTask.LoadFrom(subnodes[i]));
                }
                SubTasks.Clear();
                SubTasks.AddRange(subTsks);
                OnLoad(node);
            }
        }

        public static Task LoadFrom(ConfigNode node)
        {
            var typeName = string.Empty;
            if (node.TryGetValue(nameLabel, ref typeName))
            {
                var type = ProviderManager.GetTaskType(typeName);
                if (type != null)
                {
                    var task = (Task)Activator.CreateInstance(type);
                    if (task != null)
                    {
                        var subnodes = node.GetNodes(subTaskNodeName);
                        for (var i = 0; i < subnodes.Length; i++)
                        {
                            task.SubTasks.Add(SubTask.LoadFrom(subnodes[i]));
                        }
                        task.OnLoad(node);
                        return task;
                    }
                    else
                    {
                        // log "unable to create instance of type " + type.Name
                    }
                }
                else
                {
                    // log "unable to find type with name " + typeName
                }
            }
            else
            {
                // log "unable to find " + nameLabel + " in config
            }
            return null;
        }
    }
}
