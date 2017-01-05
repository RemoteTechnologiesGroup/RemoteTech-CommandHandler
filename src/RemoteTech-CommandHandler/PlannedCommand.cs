using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class PlannedCommand
    {
        public const string configNodeName = "PlannedCommand";

        public readonly Command command;
        public readonly Condition condition;
        public readonly Guid vesselId;

        public PlannedCommand(ConfigNode node)
        {

        }

        public PlannedCommand(Command cmd, Condition cond, Guid vesselGuid)
        {
            command = cmd;
            condition = cond;
            vesselId = vesselGuid;
        }

        public void Save(ConfigNode node)
        {
            node.AddNode("");
        }
    }
}
