using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using System.Linq;
namespace Game.Simulation
{
    public class SimRegistry
    {
        private Simulator simWorld;
        private int nextId;
        private SortedSet<ISimObject> orderedSimObjects;
        private Dictionary<string, ISimObject> simObjectsById;
        private Dictionary<ISimObject, string> idTable;

        public SimRegistry(Simulator simWorld)
        {
            this.simWorld = simWorld;
            orderedSimObjects = new SortedSet<ISimObject>(new SimObjectComparer(this));
            simObjectsById = new Dictionary<string, ISimObject>();
            idTable = new Dictionary<ISimObject, string>();
        }

        public void RegisterSimulationObject(ISimObject simulationObject)
        {
            if (idTable.ContainsKey(simulationObject)) {
                Game.Util.Utils.Log($"Simulation object {simulationObject.ToString()} already registered");
                return;
            }
            string id = GenerateId();
            idTable.Add(simulationObject, id);
            orderedSimObjects.Add(simulationObject);
            simObjectsById.Add(id, simulationObject);
            simulationObject.simWorld = simWorld;
            simulationObject.Init();
        }

        string GenerateId()
        {
            return (nextId++).ToString();
        }

        public void UnregisterSimulationObject(ISimObject simulationObject)
        {
            if (!idTable.ContainsKey(simulationObject)) {
                Game.Util.Utils.Log($"Simulation object {simulationObject.ToString()} not registered");
                return;
            }
            string id = idTable[simulationObject];
            orderedSimObjects.Remove(simulationObject);
            simObjectsById.Remove(id);
            idTable.Remove(simulationObject);
            simulationObject.simWorld = null;
        }

        public IEnumerable<ISimObject> GetOrderedSimulationObjects()
        {
            this.Log($"Getting {orderedSimObjects.Count} simulation objects: \n[\n{string.Join("\n ", orderedSimObjects.Select(s => "  " + s.ToString()))}\n]");
            return orderedSimObjects;
        }

        public ISimObject GetSimulationObject(string id)
        {
            if (!simObjectsById.TryGetValue(id, out ISimObject simulationObject)) {
                this.LogError($"Simulation object with id {id} not found", true);
                return null;
            }
            return simulationObject;
        }

        public void Clear()
        {
            orderedSimObjects.Clear();
            simObjectsById.Clear();
            idTable.Clear();
            nextId = 0;
        }

        public string GetId(ISimObject simulationObject)
        {
            if (!idTable.TryGetValue(simulationObject, out string id)) {
                this.LogError($"Simulation object {simulationObject.ToString()} not registered", true);
                return "[missing]";
            }
            return id;
        }
        private class SimObjectComparer : IComparer<ISimObject>
        {
            private readonly SimRegistry registry;

            public SimObjectComparer(SimRegistry registry)
            {
                this.registry = registry;
            }

            public int Compare(ISimObject a, ISimObject b)
            {
                if (ReferenceEquals(a, b)) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                int tickOrderCompare = a.tickOrder.CompareTo(b.tickOrder);
                if (tickOrderCompare != 0) return tickOrderCompare;

                return registry.GetId(a).CompareTo(registry.GetId(b));
            }
        }
    }
}
