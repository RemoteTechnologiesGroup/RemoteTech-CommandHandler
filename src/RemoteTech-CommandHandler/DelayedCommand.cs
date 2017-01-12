using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class DelayedCommand
    {
        public const string commandNodeName = "PlannedCommand";
        public const string fromVesselLabel = "FromVessel";
        public const string toVesselLabel = "ToVessel";
        public const string sentUTLabel = "SentUT";

        public readonly PlannedCommand command;

        private Vessel fromVessel;
        private Vessel toVessel;
        private double sentUT;
        private double receiveUT;

        public DelayedCommand(PlannedCommand cmd, Vessel from, Vessel to)
        {
            //if (toVessel == null) return;
            
            // if from vessel is null, it's Home
            command = cmd;
            fromVessel = from;
            toVessel = to;
            sentUT = Planetarium.GetUniversalTime();

            var modules = toVessel.vesselModules;
            var found = false;
            for (var i = 0; i < modules.Count; i++)
            {
                if ((modules[i] as CommNet.CommNetVessel) != null)
                {
                    found = true;
                    receiveUT = sentUT + (modules[i] as CommNet.CommNetVessel).SignalDelay;
                }
            }
            if (!found)
            {
                // log unable to locate CommNetVessel on target vessel for planned command
            }
        }

        public DelayedCommand(ConfigNode node)
        {
            if (!node.HasValue(fromVesselLabel)) return;
            if (!node.HasValue(toVesselLabel)) return;
            if (!node.HasValue(sentUTLabel)) return;
            if (!node.HasNode(commandNodeName)) return;
            // log before returning

            command = new PlannedCommand(node.GetNode(commandNodeName));
            fromVessel = FlightGlobals.FindVessel(new Guid(node.GetValue(fromVesselLabel)));
            toVessel = FlightGlobals.FindVessel(new Guid(node.GetValue(toVesselLabel)));
            if (!double.TryParse(node.GetValue(sentUTLabel), out sentUT))
            {
                // log error parsing sentUT from config node
            }
        }

        public bool isDelivered()
        {
            return Planetarium.GetUniversalTime() >= receiveUT;
        }
    }
}
