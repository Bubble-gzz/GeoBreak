using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using System.Linq;
namespace Game.Simulation
{
    public class SimRegistry
    {
        private SortedDictionary<(int tickOrder, string id), ISimulationObject> simulationObjects;
        public SimRegistry()
        {
            simulationObjects = new SortedDictionary<(int tickOrder, string id), ISimulationObject>();
        }
        public void RegisterSimulationObject(ISimulationObject simulationObject)
        {
            int tickOrder = simulationObject.GetAttribute<TickOrder>()?.Order ?? TickOrder.DefaultOrder;
            string id = (simulationObject as MonoBehaviour).GetComponentKey();
            simulationObjects.Add((tickOrder, id), simulationObject);
        }
        public IEnumerable<ISimulationObject> GetOrderedSimulationObjects()
        {
            this.Log($"Getting {simulationObjects.Count} simulation objects: \n[\n{string.Join("\n ", simulationObjects.Select(s => "  " + s.Value.ToString()))}\n]");
            return simulationObjects.Values;
        }
    }
}