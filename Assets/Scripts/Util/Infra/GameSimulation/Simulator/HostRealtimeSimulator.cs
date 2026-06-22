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
        private InputData localInputDataThisTick;
        List<TickContext> tickHistory;
        List<List<SimObjectState>> syncListOfEachTick;
        List<string> registeredThisTick;
        Dictionary<string, string> objectTypeById;

        protected override void ExtraInit()
        {
            tick = 0;
            tickHistory = new List<TickContext>();
            syncListOfEachTick = new List<List<SimObjectState>>();
            registeredThisTick = new List<string>();
            objectTypeById = new Dictionary<string, string>();
        }

        public override void RegisterObject(ISimObject simObject)
        {
            base.RegisterObject(simObject);
            string id = simRegistry.GetId(simObject);
            registeredThisTick.Add(id);
            objectTypeById[id] = simObject.GetType().FullName;
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
            CloseLogFile();
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

            TickContext tickCtx = BuildTickContext();
            tickHistory.Add(tickCtx);
            this.Log($"Tick #{tick}: Ctx = {tickCtx}");
            SimulateTick(tickCtx);
            RenderTick(Time.fixedDeltaTime);
            List<string> unregisteredIds = objectsToUnregister
                .Select(o => simRegistry.GetId(o))
                .ToList();

            TakeSyncSnapshot();
            EndTick();
            tick++;
        }
        protected override void BeforeTick()
        {
            base.BeforeTick();
            registeredThisTick.Clear();
            localInputDataThisTick = InputProcessor.Instance.ConsumeInputDataOverFrames();
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

        const int SYNC_FRAME_INTERVAL = 500;
        

        void TakeSyncSnapshot()
        {
            syncListOfEachTick.Add(new List<SimObjectState>());
            if (tick % SYNC_FRAME_INTERVAL == 0) {
                syncListOfEachTick[tick] = CollectObjectStates(tick);
            }
        }

        public GameHistory ExportGameHistory()
        {
            return new GameHistory(
                seed,
                tickHistory.ToList(),
                syncListOfEachTick.ToArray(),
                new Dictionary<string, string>(objectTypeById)
            );
        }
    }
}
