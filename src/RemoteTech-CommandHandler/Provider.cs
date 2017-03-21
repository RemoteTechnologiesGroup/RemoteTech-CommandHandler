using System;
using System.Collections.Generic;
using UnityEngine;

namespace RemoteTech.CommandHandler
{
    public abstract class Provider
    {
        public abstract List<Type> GetTasks(Part part);
        public abstract List<Type> GetTasks(Part part, List<Type> list);
        public abstract List<Type> GetConditions(Part part);
        public abstract List<Type> GetConditions(Part part, List<Type> list);
        //public abstract List<Task> GetActions(Vessel vessel);
        //public abstract List<Condition> GetConditions(Vessel vessel);
    }
}