using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class DelayedCommand
    {
        private const string commandNodeName = "PlannedCommand";
        private const string fromVesselLabel = "FromVessel";
        private const string toVesselLabel = "ToVessel";
        private const string sentUTLabel = "SentUT";
        private const string guidFromat = "N";

        public readonly PlannedCommand command;
        public readonly Vessel fromVessel;
        public readonly Vessel toVessel;

        private double sentUT;
        private double receiveUT;

        public bool Delivered
        {
            get
            {
                return Planetarium.GetUniversalTime() >= receiveUT;
            }
        }

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

            command = PlannedCommand.Load(node.GetNode(commandNodeName));
            fromVessel = FlightGlobals.FindVessel(new Guid(node.GetValue(fromVesselLabel)));
            toVessel = FlightGlobals.FindVessel(new Guid(node.GetValue(toVesselLabel)));
            if (!double.TryParse(node.GetValue(sentUTLabel), out sentUT))
            {
                // log error parsing sentUT from config node
            }
        }

        public void Save(ConfigNode node)
        {
            node.AddValue(fromVesselLabel, fromVessel.id.ToString(guidFromat));
            node.AddValue(toVesselLabel, toVessel.id.ToString(guidFromat));
            node.AddValue(sentUTLabel, sentUT);
            command.Save(node.AddNode(commandNodeName));
        }
    }
}
