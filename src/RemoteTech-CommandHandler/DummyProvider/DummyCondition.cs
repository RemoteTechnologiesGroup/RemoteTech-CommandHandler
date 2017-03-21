using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech.CommandHandler.DummyProvider
{
    class DummyCondition : Condition
    {
        public override void Configure()
        {
            Debug.Log("[DummyCondition] Configure()");
            ScreenMessages.PostScreenMessage(string.Format("[{0}]: Dummy Conditiond configured.", part.partInfo.title), 4f, ScreenMessageStyle.UPPER_CENTER);
        }
    }
}
