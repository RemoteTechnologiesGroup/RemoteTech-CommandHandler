using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech.CommandHandler
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class CommandManager : MonoBehaviour
    {
        private const string plannedCommandsVesselNodeName = "VesselCommands";
        private const string plannedCommandsVesselGuidName = "VesselGuid";
        private const string plannedCommandNodeName = "PlannedCommand";
        private const string delayedCommandNodeName = "DelayedCommand";

        private const int initialListSize = 10;

        public static CommandManager Instance
        {
            get;
            private set;
        }

        private Dictionary<Vessel, List<PlannedCommand>> plannedCommands = new Dictionary<Vessel, List<PlannedCommand>>(initialListSize);
        private List<DelayedCommand> delayedCommands = new List<DelayedCommand>(initialListSize);
        private static List<Guid> usedCommandGuids = new List<Guid>(initialListSize);
        private static List<Guid> issuedCommandGuids = new List<Guid>(initialListSize);
        private bool needToLoad = true;

        public Guid NewCommandId()
        {
            var id = Guid.NewGuid();
            while (usedCommandGuids.Contains(id) || issuedCommandGuids.Contains(id))
            {
                id = Guid.NewGuid();
            }
            issuedCommandGuids.Add(id);
            return id;
        }

        public void Awake()
        {
            GameEvents.onGameStateLoad.Add(new EventData<ConfigNode>.OnEvent(GameLoadEvent));
        }

        public void OnDestroy()
        {
            GameEvents.onGameStateLoad.Remove(new EventData<ConfigNode>.OnEvent(GameLoadEvent));
        }

        /// <summary>
        /// Adds command to the CommandPlanner if capture is on, otherwise queues the command for active vessel
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public void AddTaskAsCommand(Task task)
        {
            // check if CommandPlanner has "capture" enabled, if on, pass to planner, else
            // check active vessel, if none, drop the command, otherwise PlanCommand for active vessel with no condition
        }

        public void PlanCommand(Task command, Condition condition, bool enabled, bool oneShot, Vessel fromVessel, Vessel toVessel)
        {
            PlanCommand(new PlannedCommand(command, condition, enabled, oneShot), fromVessel, toVessel);
        }

        public void PlanCommand(PlannedCommand planCommand, Vessel fromVessel, Vessel toVessel)
        {
            // if fromVessel is null, it's Home
            // if toVessel is null, it's active vessel
            if (toVessel == null)
            {
                if (FlightGlobals.ActiveVessel != null)
                {
                    toVessel = FlightGlobals.ActiveVessel;
                }
                else
                {
                    // log cannot determine target vessel for planned command
                    return;
                }
            }
            delayedCommands.Add(new DelayedCommand(planCommand, fromVessel, toVessel));
            if (issuedCommandGuids.Contains(planCommand.Id))
            {
                issuedCommandGuids.Remove(planCommand.Id);
            }
            usedCommandGuids.Add(planCommand.Id);
        }

        public void Load(ConfigNode node)
        {
            if (!needToLoad) return;
            // log before returning

            plannedCommands.Clear();
            var nodes = node.GetNodes(plannedCommandsVesselNodeName);
            for (var i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].HasValue(plannedCommandsVesselGuidName))
                {
                    var list = new List<PlannedCommand>(initialListSize);
                    plannedCommands.Add(FlightGlobals.FindVessel(new Guid(nodes[i].GetValue(plannedCommandsVesselGuidName))), list);
                    var commands = nodes[i].GetNodes(plannedCommandNodeName);
                    for (var j = 0; j < commands.Length; j++)
                    {
                        list.Add(PlannedCommand.Load(commands[j]));
                    }
                }
            }

            delayedCommands.Clear();
            nodes = node.GetNodes(delayedCommandNodeName);
            for (var i = 0; i < nodes.Length; i++)
            {
                delayedCommands.Add(new DelayedCommand(nodes[i]));
            }
            needToLoad = false;
        }

        public void Save(ConfigNode node)
        {
            var vessels = new List<Vessel>(plannedCommands.Keys);
            for (var i = 0; i < vessels.Count; i++)
            {
                var vesselNode = node.AddNode(plannedCommandsVesselNodeName);
                for (var j = 0; j < plannedCommands[vessels[i]].Count; j++)
                {
                    plannedCommands[vessels[i]][j].Save(vesselNode.AddNode(plannedCommandNodeName));
                }
            }
            for (var i = 0; i < delayedCommands.Count; i++)
            {
                delayedCommands[i].Save(node.AddNode(delayedCommandNodeName));
            }
        }

        private void GameLoadEvent(ConfigNode node)
        {
            needToLoad = true;
        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedScene > GameScenes.CREDITS)
            {
                CheckDelayedCommands();
                CheckPlannedCommands();
            }
        }

        private void CheckPlannedCommands()
        {
            if (FlightGlobals.ActiveVessel != null)
            {
                var list = plannedCommands[FlightGlobals.ActiveVessel];
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].Enabled && (list[i].Condition == null || list[i].Condition.IsFulfilled) && list[i].Task.State == Task.TaskState.Ready)
                    {
                        list[i].Task.Activate();
                    }
                    if (list[i].Task.State >= Task.TaskState.Finished)
                    {
                        if (list[i].Task.State != Task.TaskState.Failed)
                        {
                            if (list[i].OneShot)
                            {
                                list.RemoveAt(i);
                            }
                            else
                            {
                                list[i].Enabled = false;
                            }
                        }
                        else
                        {
                            // what to do with a failed task?
                        }
                    }
                }
            }
        }

        private void CheckDelayedCommands()
        {
            for (var i = delayedCommands.Count - 1; i >= 0; i--)
            {
                if (delayedCommands[i].Delivered)
                {
                    if (plannedCommands[delayedCommands[i].toVessel] == null)
                    {
                        plannedCommands.Add(delayedCommands[i].toVessel, new List<PlannedCommand>(initialListSize));
                    }
                    plannedCommands[delayedCommands[i].toVessel].Add(delayedCommands[i].command);
                    delayedCommands.RemoveAt(i);
                }
            }
        }
    }
}
