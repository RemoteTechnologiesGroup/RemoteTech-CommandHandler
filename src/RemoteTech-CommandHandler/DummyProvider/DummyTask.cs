using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech.CommandHandler.DummyProvider
{
    class DummyTask : Task
    {
        protected DummyTask()
        {
            State = TaskState.Ready;
            DisplayName = "Dummy Task";
        }

        public override void Activate()
        {
            Debug.Log("[DummyTask] Activate()");
            ScreenMessages.PostScreenMessage(string.Format("[{0}]: Dummy Task activated.", part.partInfo.title), 4f, ScreenMessageStyle.UPPER_CENTER);
            State = TaskState.Finished;
        }

        public override void Configure()
        {
            Debug.Log("[DummyTask] Configure()");
            ScreenMessages.PostScreenMessage(string.Format("[{0}]: Dummy Task configured.", part.partInfo.title), 4f, ScreenMessageStyle.UPPER_CENTER);
        }
    }
}
