using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using System.Linq;
namespace Game.Simulation
{
    public class SimRegistry
    {
        private SortedDictionary<(int tickOrder, string id), ISimulationObject> orderedSimObjects;
        private Dictionary<string, ISimulationObject> simObjectsById;
        public SimRegistry()
        {
            orderedSimObjects = new SortedDictionary<(int tickOrder, string id), ISimulationObject>();
            simObjectsById = new Dictionary<string, ISimulationObject>();
        }
        public void RegisterSimulationObject(ISimulationObject simulationObject)
        {
            int tickOrder = simulationObject.GetAttribute<TickOrder>()?.Order ?? TickOrder.DefaultOrder;
            string id = simulationObject.id;
            orderedSimObjects.Add((tickOrder, id), simulationObject);
            simObjectsById.Add(id, simulationObject);
        }
        public IEnumerable<ISimulationObject> GetOrderedSimulationObjects()
        {
            this.Log($"Getting {orderedSimObjects.Count} simulation objects: \n[\n{string.Join("\n ", orderedSimObjects.Select(s => "  " + s.Value.ToString()))}\n]");
            return orderedSimObjects.Values;
        }
        public ISimulationObject GetSimulationObject(string id)
        {
            return simObjectsById[id];
        }
        public void Clear()
        {
            orderedSimObjects.Clear();
            simObjectsById.Clear();
        }
    }
}