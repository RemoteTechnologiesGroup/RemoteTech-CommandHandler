using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    public class RemoteTechCmdHandlerParams : GameParameters.CustomParameterNode
    {
        [GameParameters.CustomStringParameterUI("", autoPersistance = false, lines = 3)]
        public string description = "To uninstall Command Manager, delete 'RemoteTech-CommandHandler' from RemoteTech's main folder";
        
        [GameParameters.CustomParameterUI("Add Alarms to Kerbal Alarm Clock", toolTip = "ON: The flight computer will automatically add alarms to the Kerbal Alarm Clock mod for burn and maneuver commands.\nThe alarm goes off 3 minutes before the command executes.\nOFF: No alarms are added to Kerbal Alarm Clock")]
        public bool AutoInsertKaCAlerts = true;

        [GameParameters.CustomParameterUI("Throttle time warp", toolTip = "ON: The flight computer will automatically stop time warp a few seconds before executing a queued command.\nOFF: The player is responsible for controlling time warp during scheduled actions.")]
        public bool ThrottleTimeWarp = true;

        [GameParameters.CustomParameterUI("Throttle to zero on loss of connection", toolTip = "ON: The flight computer cuts the thrust if you lose connection to Mission Control.\nOFF: The throttle is not adjusted automatically.")]
        public bool ThrottleZeroOnNoConnection = true;

        public override string DisplaySection
        {
            get
            {
                return "RemoteTech";
            }
        }

        public override GameParameters.GameMode GameMode
        {
            get
            {
                return GameParameters.GameMode.ANY;
            }
        }

        public override bool HasPresets
        {
            get
            {
                return false;
            }
        }

        public override string Section
        {
            get
            {
                return "RemoteTech";
            }
        }

        public override int SectionOrder
        {
            get
            {
                return 1;
            }
        }

        public override string Title
        {
            get
            {
                return "Command Manager";
            }
        }
    }
}
