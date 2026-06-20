using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Util;
using System.Linq;

namespace Game.Simulation
{
    public abstract class Simulator : MonoBehaviour
    {
        protected int seed;
        protected IRandomGenerator rng;
        protected SimRegistry simRegistry;
        protected void Init(int seed)
        {
            this.seed = seed;
            rng = new RandomGenerator(seed);
            simRegistry = new SimRegistry();
            FetchSimulatedObjectsInScene();
            ExtraInit();
        }
        protected virtual void ExtraInit() {}
        protected void FetchSimulatedObjectsInScene()
        {
            simRegistry.Clear();
            List<ISimulationObject> simulationObjectsList = new List<ISimulationObject>();
            MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is ISimulationObject simulationObject)
                {
                    simulationObjectsList.Add(simulationObject);
                }
            }
            foreach (var simulationObject in simulationObjectsList)
            {
                simRegistry.RegisterSimulationObject(simulationObject);
            }
            this.Log($"Fetched {simulationObjectsList.Count} simulation objects: [{string.Join(", ", simulationObjectsList.Select(s => s.ToString()))}]");
        }
        public void RegisterSimulationObject(ISimulationObject simulationObject)
        {
            simRegistry.RegisterSimulationObject(simulationObject);
        }
    }
    public interface ISimulationObject
    {
        string id { get; }
        void Init();
        void Tick(TickContext tickCtx);
        void SerializeState(StateWriter writer);
        void DeserializeState(StateReader reader);
        void Render(float deltaTime);
    }
    public class SimObjectState{
        public int tick;
        public string objectId;
        public byte[] data;
        public SimObjectState(int tick, string objectId, byte[] data)
        {
            this.tick = tick;
            this.objectId = objectId;
            this.data = data;
        }
    }
}