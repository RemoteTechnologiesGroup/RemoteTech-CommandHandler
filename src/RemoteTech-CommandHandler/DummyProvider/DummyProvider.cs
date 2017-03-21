using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler.DummyProvider
{
    class DummyProvider : Provider
    {
        public override List<Type> GetConditions(Part part)
        {
            return GetConditions(part, null);
        }

        public override List<Type> GetConditions(Part part, List<Type> list)
        {
            if (list == null)
            {
                list = new List<Type>(1);
            }
            else
            {
                list.Clear();
            }
            list.Add(typeof(DummyCondition));
            return list;
        }

        public override List<Type> GetTasks(Part part)
        {
            return GetTasks(part, null);
        }

        public override List<Type> GetTasks(Part part, List<Type> list)
        {
            if (list == null)
            {
                list = new List<Type>(1);
            }
            else
            {
                list.Clear();
            }
            list.Add(typeof(DummyTask));
            return list;
        }
    }
}
