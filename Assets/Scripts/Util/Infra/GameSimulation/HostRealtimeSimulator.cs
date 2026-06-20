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
        protected override void ExtraInit()
        {
            tickCount = 0;
            tickHistory = new List<TickContext>();
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
        void BeforeTick()
        {
            localInputDataThisTick = InputProcessor.Instance.ConsumeInputDataOverFrames();
        }
        void AfterTick()
        {
            tickCount++;
        }
        void TickSimulation()
        {
            TickContext tickCtx = BuildTickContext();
            this.Log($"Tick #{tickCount}: Ctx = {tickCtx}");
            foreach (var simulationObject in simRegistry.GetOrderedSimulationObjects())
            {
                simulationObject.Tick(tickCtx);
            }
            tickHistory.Add(tickCtx);
        }
        void TickRender()
        {
            
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
        public GameHistory ExportGameHistory()
        {
            return new GameHistory(seed, tickHistory.ToList());
        }
    }
}