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
        List<FrameRecord> frameRecords;
        List<int> registeredThisTick;
        Dictionary<int, string> objectTypeById;
        protected override void ExtraInit()
        {
            tickCount = 0;
            tickHistory = new List<TickContext>();
            stateHistory = new List<SimObjectState>();
            frameRecords = new List<FrameRecord>();
            registeredThisTick = new List<int>();
            objectTypeById = new Dictionary<int, string>();
        }
        public override void RegisterObject(ISimObject simObject)
        {
            base.RegisterObject(simObject);
            int id = simRegistry.GetId(simObject);
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
            registeredThisTick.Clear();
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
            List<SimObjectState> debugObjectStates = new List<SimObjectState>();
            TakeDebugSnapshot(debugObjectStates);
            List<int> unregisteredIds = objectsToUnregister
                .Select(o => simRegistry.GetId(o))
                .ToList();
            frameRecords.Add(new FrameRecord(
                tickCount,
                debugObjectStates,
                new List<int>(registeredThisTick),
                unregisteredIds
            ));
            TakeSyncSnapshot();
            UnregisterQueuedObjects();
            tickCount++;
        }
        void TakeDebugSnapshot(List<SimObjectState> snapshots)
        {
            foreach (var simulationObject in simListSnapshot)
            {
                int objectId = simRegistry.GetId(simulationObject);
                StateWriter writer = new StateWriter();
                simulationObject.SerializeState(writer);
                byte[] data = writer.ToArray();
                snapshots.Add(new SimObjectState(tickCount, objectId, data));
            }
        }
        void TakeSyncSnapshot()
        {
            if (tickCount % SYNC_FRAME_INTERVAL == 0) {
                foreach (var simulationObject in simListSnapshot)
                {
                    int objectId = simRegistry.GetId(simulationObject);
                    StateWriter writer = new StateWriter();
                    simulationObject.SerializeState(writer);
                    byte[] data = writer.ToArray();
                    stateHistory.Add(new SimObjectState(tickCount, objectId, data));
                }
            }
        }
        public GameHistory ExportGameHistory()
        {
            return new GameHistory(
                seed,
                tickHistory.ToList(),
                stateHistory.ToList(),
                frameRecords.Select(f => f.Clone()).ToList(),
                new Dictionary<int, string>(objectTypeById)
            );
        }
    }
}