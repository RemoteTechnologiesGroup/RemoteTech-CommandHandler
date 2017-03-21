using System;
using System.Collections.Generic;
using UnityEngine;

namespace RemoteTech.CommandHandler
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class ProviderManager : MonoBehaviour
    {
        private const int initListSize = 1;
        private const int initDictSize = 10;

        private static Dictionary<string, Type> Tasks = new Dictionary<string, Type>(initDictSize);
        private static Dictionary<string, Type> Conditions = new Dictionary<string, Type>(initDictSize);
        private static List<Provider> Providers = new List<Provider>(initListSize);

        private void Awake()
        {
            Debug.Log("[ProviderManager] Awake()");
            AssemblyLoader.loadedAssemblies.TypeOperation(delegate (Type t)
            {
                if (!t.IsAbstract)
                {
                    if (t.IsSubclassOf(typeof(Task)) && !t.IsAbstract)
                    {
                        Tasks.Add(t.Name, t);
                    }
                    if (t.IsSubclassOf(typeof(Condition)) && !t.IsAbstract)
                    {
                        Conditions.Add(t.Name, t);
                    }
                    if (t.IsSubclassOf(typeof(Provider)) && !t.IsAbstract)
                    {
                        Providers.Add((Provider)Activator.CreateInstance(t));
                    }
                }
            });
            var list = new List<string>();
            foreach (var provider in Providers)
            {
                list.Add(provider.GetType().Name);
            }
            Debug.Log("[ProviderManager] Found Providers: " + string.Join(", ", list.ToArray()));
            Debug.Log("[ProviderManager] Found Tasks: " + string.Join(", ", (new List<string>(Tasks.Keys)).ToArray()));
            Debug.Log("[ProviderManager] Found Conditions: " + string.Join(", ", (new List<string>(Conditions.Keys)).ToArray()));
        }

        public static Type GetTaskType(string name)
        {
            return Tasks[name];
        }

        public static Type GetConditionType(string name)
        {
            return Conditions[name];
        }

        public static List<Type> ValidTasksFor(Part part)
        {
            return ValidTasksFor(part, null);
        }

        public static List<Type> ValidTasksFor(Part part, List<Type> list)
        {
            if (list == null)
            {
                list = new List<Type>();
            }
            else
            {
                list.Clear();
            }
            var tmpList = new List<Type>();
            foreach (var provider in Providers)
            {
                list.AddRange(provider.GetTasks(part, tmpList));
            }
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (Tasks[list[i].Name] == null)
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }

        public static List<Type> ValidConditionsFor(Part part)
        {
            return ValidConditionsFor(part, null);
        }

        public static List<Type> ValidConditionsFor(Part part, List<Type> list)
        {
            if (list == null)
            {
                list = new List<Type>();
            }
            else
            {
                list.Clear();
            }
            var tmpList = new List<Type>();
            foreach (var provider in Providers)
            {
                list.AddRange(provider.GetConditions(part, tmpList));
            }
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (Conditions[list[i].Name] == null)
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }
    }
}
