using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;
using Game.System;
using System.Linq;
namespace Game.Simulation
{
    public class HostRealtimeSimulator : Simulator
    {
        private int tickCount;
        private InputData localInputDataThisTick;
        List<TickContext> tickHistory;
        List<SimObjectState> stateHistory;
        protected override void ExtraInit()
        {
            tickCount = 0;
            tickHistory = new List<TickContext>();
            stateHistory = new List<SimObjectState>();
        }
        public void RunWithRandomSeed()
        {
            Run(new global::System.Random().Next());
        }
        Coroutine simulationCoroutine;
        public void Run(int seed)
        {
            Init(seed);
            Stop();
            simulationCoroutine = StartCoroutine(RunSimulationCoroutine());
        }
        public void Stop()
        {
            if (simulationCoroutine == null) return;
            StopCoroutine(simulationCoroutine);
            simulationCoroutine = null;
        }
        IEnumerator RunSimulationCoroutine()
        {
            while (true)
            {
                Tick();
                yield return new WaitForFixedUpdate();
            }
        }
        void Tick()
        {
            BeforeTick();
            TickSimulation();
            TickRender();
            AfterTick();
        }
        List<ISimObject> simListSnapshot;
        void BeforeTick()
        {
            localInputDataThisTick = InputProcessor.Instance.ConsumeInputDataOverFrames();
            simListSnapshot = simRegistry.GetOrderedSimulationObjects().ToList();
        }
        void TickSimulation()
        {
            TickContext tickCtx = BuildTickContext();
            tickHistory.Add(tickCtx);
            this.Log($"Tick #{tickCount}: Ctx = {tickCtx}");
            foreach (var simulationObject in simListSnapshot)
            {
                simulationObject.Tick(tickCtx);
            }
        }
        TickContext BuildTickContext()
        {
            TickContext tickCtx = new TickContext();
            tickCtx.deltaTime = Time.fixedDeltaTime;
            tickCtx.rng = rng;
            tickCtx.inputDatas = new List<InputData> { localInputDataThisTick };
            tickCtx.gameSettings = new List<GameSettings> { GameSettings.LocalInstance };
            return tickCtx;
        }
        void TickRender()
        {
            foreach (var simulationObject in simListSnapshot)
            {
                simulationObject.Render(Time.fixedDeltaTime);
            }
        }
        const int SYNC_FRAME_INTERVAL = 500;
        void AfterTick()
        {
            TakeSnapshot();
            UnregisterQueuedObjects();
            tickCount++;
        }
        void TakeSnapshot()
        {
            if (tickCount % SYNC_FRAME_INTERVAL == 0) {
                foreach (var simulationObject in simListSnapshot)
                {
                    int objectId = simRegistry.GetId(simulationObject);
                    StateWriter writer = new StateWriter();
                    simulationObject.SerializeState(writer);
                    byte[] data = writer.ToArray();
                    SimObjectState snapshot = new SimObjectState(tickCount, objectId, data);
                    stateHistory.Add(snapshot);
                }
            }
        }
        public GameHistory ExportGameHistory()
        {
            return new GameHistory(
                this,
                seed,
                tickHistory.ToList(),
                stateHistory.ToList()
            );
        }
    }
}