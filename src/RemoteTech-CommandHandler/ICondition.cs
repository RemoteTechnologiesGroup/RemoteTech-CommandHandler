using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    public interface ICondition
    {
        bool IsFulfilled
        {
            get;
        }
        string providerName
        {
            get;
        }
        void Save(ConfigNode node);
    }
}
