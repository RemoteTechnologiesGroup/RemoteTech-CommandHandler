using System;
using System.Collections.Generic;

namespace RemoteTech.CommandHandler
{
    public abstract class Condition
    {
        private const string nameLabel = "name";
        private const string subConditionNodeName = "SubCondition";

        public Part part;

        public virtual string DisplayName { get; protected set; }
        public virtual bool IsFulfilled { get; protected set; }
        public virtual List<SubCondition> SubConditions { get; protected set; }

        public abstract void Configure();

        public virtual void OnSave(ConfigNode node) { }

        public virtual void OnLoad(ConfigNode node) { }

        protected Condition()
        {
            SubConditions = new List<SubCondition>();
        }
        
        public void Save(ConfigNode node)
        {

            node.AddValue(nameLabel, GetType().Name);
            OnSave(node);
            var subConds = SubConditions;
            for (var i = 0; i < subConds.Count; i++)
            {
                subConds[i].Save(node.AddNode(subConditionNodeName));
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
                var subnodes = node.GetNodes(subConditionNodeName);
                var subTsks = new List<SubCondition>(subnodes.Length);
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subTsks.Add(SubCondition.LoadFrom(subnodes[i]));
                }
                SubConditions.Clear();
                SubConditions.AddRange(subTsks);
                OnLoad(node);
            }
        }

        public static Condition LoadFrom(ConfigNode node)
        {
            var typeName = string.Empty;
            if (node.TryGetValue(nameLabel, ref typeName))
            {
                var type = ProviderManager.GetTaskType(typeName);
                if (type != null)
                {
                    var cond = (Condition)Activator.CreateInstance(type);
                    if (cond != null)
                    {
                        var subnodes = node.GetNodes(subConditionNodeName);
                        for (var i = 0; i < subnodes.Length; i++)
                        {
                            cond.SubConditions.Add(SubCondition.LoadFrom(subnodes[i]));
                        }
                        cond.OnLoad(node);
                        return cond;
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
