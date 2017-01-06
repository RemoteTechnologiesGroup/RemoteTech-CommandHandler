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
        string ProviderName
        {
            get;
        }
        ICondition[] SubConditions
        {
            get;
        }
        void Save(ConfigNode node);
        void AddSubCondition(ICondition cmd);
    }
}
