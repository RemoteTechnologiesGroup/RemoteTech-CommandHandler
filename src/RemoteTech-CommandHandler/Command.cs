using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteTech.CommandHandler
{
    class Command
    {
        public const string commandNodeName = "Command";

        public static void Save(ICommand cmd, ConfigNode node)
        {
            node.AddValue(ProviderManager.providerConfigLabelName, cmd.ProviderName);
            var extNode = node.AddNode(ProviderManager.providerDataNodeName);
            cmd.Save(extNode);
            var subcmd = cmd.SubCommands;
            for(var i = 0; i < subcmd.Length; i++)
            {
                extNode = node.AddNode(commandNodeName);
                subcmd[i].Save(extNode);
            }
        }

        public static ICommand Load(ConfigNode node)
        {
            string providerName = string.Empty;
            ConfigNode extNode = null;
            if (node.TryGetValue(ProviderManager.providerConfigLabelName, ref providerName) && node.TryGetNode(ProviderManager.providerDataNodeName, ref extNode))
            {
                var subnodes = node.GetNodes(commandNodeName);
                var subcmd = new ICommand[subnodes.Length];
                for (var i = 0; i < subnodes.Length; i++)
                {
                    subcmd[i] = Load(subnodes[i]);
                }
                var provider = ProviderManager.FindProvider(providerName);
                if (provider != null)
                {
                    var cmd = provider.LoadCommand(extNode);
                    for (var i=0; i<subcmd.Length; i++)
                    {
                        cmd.AddSubCommand(subcmd[i]);
                    }
                    return cmd;
                }
                else
                {
                    // log "Unable to find provider with name 'providerName'"
                }
            }
            else
            {
                if(providerName == string.Empty)
                {
                    // log "Unable to get provider name from config node"
                }
                if (extNode == null)
                {
                    // log "Unable to get provider data from config node"
                }

            }
            return null;
        }
    }
}
