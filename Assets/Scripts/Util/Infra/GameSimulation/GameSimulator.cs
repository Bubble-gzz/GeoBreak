using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Util;
using System.Linq;

namespace Game.Simulation
{
    public class GameSimulator : MonoBehaviour
    {
        public IRandomGenerator rng;
        public void Init(int seed)
        {
            rng = new RandomGenerator(seed);
        }
        public void InitWithRandomSeed()
        {
            Init(new global::System.Random().Next());
        }
        void Awake()
        {
            InitWithRandomSeed();
        }
        public void RunSimualtion()
        {
            tickCount = 0;
            FetchSimulatedObjectsInScene();
            StartCoroutine(RunSimulationCoroutine());
        }
        void FetchSimulatedObjectsInScene()
        {
            simulationObjects = new List<ISimulationObject>();
            MonoBehaviour[] behaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is ISimulationObject simulationObject)
                {
                    simulationObjects.Add(simulationObject);
                    simulationObject.Init();
                }
            }
            Log($"Fetched {simulationObjects.Count} simulation objects: [{string.Join(", ", simulationObjects.Select(s => s.ToString()))}]");
        }
        IEnumerator RunSimulationCoroutine()
        {
            while (true)
            {
                Tick();
                yield return new WaitForFixedUpdate();
            }
        }
        List<ISimulationObject> simulationObjects;
        int tickCount;
        void Tick()
        {
            TickCtx tickCtx = BuildTickCtx();
            Log($"Tick #{tickCount}: Ctx = {tickCtx}");
            foreach (var simulationObject in simulationObjects)
            {
                simulationObject.Tick(tickCtx);
            }
            tickCount++;
        }
        TickCtx BuildTickCtx()
        {
            TickCtx tickCtx = new TickCtx();
            tickCtx.deltaTime = Time.deltaTime;
            tickCtx.rng = rng;
            tickCtx.inputDatas = new List<InputData> { InputManager.GetInputData() };
            tickCtx.gameSettings = new List<GameSettings> { GameSettings.LocalInstance };
            return tickCtx;
        }
        public void RegisterSimulationObject(ISimulationObject simulationObject)
        {
            if (simulationObjects == null) simulationObjects = new List<ISimulationObject>();
            simulationObjects.Add(simulationObject);
        }
        void Log(string msg)
        {
            Debug.Log($"[GameSimulator] " + msg);
        }
    }
    public interface ISimulationObject
    {
        void Tick(TickCtx tickCtx);
        void Init();
    }
    public interface IRenderObject<T>
    {
        void Render(T data, float deltaTime);
    }

}