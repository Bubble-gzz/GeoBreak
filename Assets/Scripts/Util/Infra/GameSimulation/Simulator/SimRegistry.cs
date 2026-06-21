using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using System.Linq;
namespace Game.Simulation
{
    public class SimRegistry
    {
        private ISimWorld simWorld;
        int nextId;
        private SortedSet<ISimObject> orderedSimObjects;
        private Dictionary<int, ISimObject> simObjectsById;
        private Dictionary<ISimObject, int> idTable;
        public SimRegistry(ISimWorld simWorld)
        {
            this.simWorld = simWorld;
            nextId = 0;
            orderedSimObjects = new SortedSet<ISimObject>(new SimObjectComparer(this));
            simObjectsById = new Dictionary<int, ISimObject>();
            idTable = new Dictionary<ISimObject, int>();
        }
        public void RegisterSimulationObject(ISimObject simulationObject)
        {
            if (idTable.ContainsKey(simulationObject)) {
                Game.Util.Utils.Log($"Simulation object {simulationObject.ToString()} already registered");
                return;
            }
            int id = nextId++;
            idTable.Add(simulationObject, id);
            orderedSimObjects.Add(simulationObject);
            simObjectsById.Add(id, simulationObject);
            simulationObject.simWorld = simWorld;
            simulationObject.Init();
        }
        public void UnregisterSimulationObject(ISimObject simulationObject)
        {
            if (!idTable.ContainsKey(simulationObject)) {
                Game.Util.Utils.Log($"Simulation object {simulationObject.ToString()} not registered");
                return;
            }
            int id = idTable[simulationObject];
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
        public ISimObject GetSimulationObject(int id)
        {
            return simObjectsById[id];
        }
        public void Clear()
        {
            orderedSimObjects.Clear();
            simObjectsById.Clear();
            idTable.Clear();
        }
        public int GetId(ISimObject simulationObject)
        {
            return idTable[simulationObject];
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