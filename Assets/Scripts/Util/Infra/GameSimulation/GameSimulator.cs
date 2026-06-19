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
        private IRandomGenerator rng;
        private SimRegistry simRegistry;
        public void Init(int seed)
        {
            rng = new RandomGenerator(seed);
            simRegistry = new SimRegistry();
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
        IEnumerator RunSimulationCoroutine()
        {
            while (true)
            {
                Tick();
                yield return new WaitForFixedUpdate();
            }
        }
        int tickCount;
        InputData localInputDataThisTick;
        void Tick()
        {
            BeforeTick();

            TickContext tickCtx = BuildTickCtx();
            this.Log($"Tick #{tickCount}: Ctx = {tickCtx}");
            foreach (var simulationObject in simRegistry.GetSimulationObjects())
            {
                simulationObject.Tick(tickCtx);
            }

            AfterTick();
        }
        void BeforeTick()
        {
            localInputDataThisTick = InputProcessor.Instance.ConsumeInputDataOverFrames();
        }
        void AfterTick()
        {
            tickCount++;
        }
        TickContext BuildTickCtx()
        {
            TickContext tickCtx = new TickContext();
            tickCtx.deltaTime = Time.fixedDeltaTime;
            tickCtx.rng = rng;
            tickCtx.inputDatas = new List<InputData> { localInputDataThisTick };
            tickCtx.gameSettings = new List<GameSettings> { GameSettings.LocalInstance };
            return tickCtx;
        }
        public void RegisterSimulationObject(ISimulationObject simulationObject)
        {
            simRegistry.RegisterSimulationObject(simulationObject);
        }
    }
    public interface ISimulationObject
    {
        void Tick(TickContext tickCtx);
        void Init();
    }
}