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

        public static ProviderManager Instance
        {
            get;
            private set;
        }

        public bool RegisterProvider(IProvider provider)
        {
            return true;
        }
        public IProvider FindProvider(string name)
        {
            return null;
        }
    }
}
