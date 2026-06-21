using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using System.Linq;

namespace Game.Simulation
{
    public static class Utils
    {
        public static List<SimMonobehaviour> FetchAllSimObjectsInScene()
        {
            return FetchSimObjectsUnder(null);
        }
        public static List<SimMonobehaviour> FetchSimObjectsUnder(Transform root)
        {
            List<SimMonobehaviour> simulationObjectsList = new List<SimMonobehaviour>();
            MonoBehaviour[] behaviours = root == null
                ? MonoBehaviour.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                : root.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is SimMonobehaviour simulationObject)
                {
                    simulationObjectsList.Add(simulationObject);
                }
            }
            simulationObjectsList.Sort(
                (a, b) => a.GetComponentKey().CompareTo(b.GetComponentKey())
            );
            Game.Util.Utils.Log($"Fetched {simulationObjectsList.Count} simulation objects: [{string.Join(", ", simulationObjectsList.Select(s => s.ToString()))}]");
            return simulationObjectsList;
        }
    }
}