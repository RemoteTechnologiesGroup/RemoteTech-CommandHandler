using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RemoteTech.CommandHandler
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    class ProviderManager : MonoBehaviour
    {
        public const string providerConfigLabelName = "provider";
        public const string providerDataNodeName = "providerData";

        private static Dictionary<String, IProvider> providers;

        public static bool RegisterProvider(string name,IProvider provider)
        {
            if (!providers.ContainsKey(name))
            {
                providers.Add(name, provider);
                return true;
            }
            return false;
        }

        public static IProvider FindProvider(string name)
        {
            return providers[name];
        }
    }
}
