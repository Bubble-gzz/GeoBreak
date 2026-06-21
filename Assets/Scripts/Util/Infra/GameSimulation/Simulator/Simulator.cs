using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Util;
using System.Linq;
using System;
namespace Game.Simulation
{
    public abstract class Simulator : MonoBehaviour, ISimWorld
    {
        protected int seed;
        protected IRandomGenerator rng;
        protected SimRegistry simRegistry;
        protected List<ISimObject> objectsToUnregister = new List<ISimObject>();
        protected void Init(int seed)
        {
            this.seed = seed;
            rng = new RandomGenerator(seed);
            simRegistry = new SimRegistry(this);
            var simObjects = Utils.FetchAllSimObjectsInScene();
            foreach (var simObject in simObjects)
            {
                simRegistry.RegisterSimulationObject(simObject);
            }
            ExtraInit();
        }
        protected virtual void ExtraInit() {}
        public virtual void RegisterObject(ISimObject simObject) {
            simRegistry.RegisterSimulationObject(simObject);
        }
        public virtual void UnregisterObject(ISimObject simObject) {
            objectsToUnregister.Add(simObject);
        }
        protected void UnregisterQueuedObjects()
        {
            foreach (var simObject in objectsToUnregister)
            {
                simRegistry.UnregisterSimulationObject(simObject);
            }
            objectsToUnregister.Clear();
        }
    }
    public interface ISimObject
    {
        ISimWorld simWorld { get; set; }
        int tickOrder { get; }
        void Init();
        void Tick(TickContext tickCtx);
        void SerializeState(StateWriter writer);
        void DeserializeState(StateReader reader);
        void Render(float deltaTime);
    }
    public interface ISimWorld
    {
        void RegisterObject(ISimObject simObject);
        void UnregisterObject(ISimObject simObject);
    }
    public class SimObjectState{
        public int tick;
        public int objectId;
        public byte[] data;
        public SimObjectState(int tick, int objectId, byte[] data)
        {
            this.tick = tick;
            this.objectId = objectId;
            this.data = data;
        }
    }
}