using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Game.Simulation
{
    public class PlaybackSimulator : Simulator
    {
        private GameHistory gameHistory;
        List<TickContext> tickHistory => gameHistory.tickHistory;
        List<SimObjectState> stateHistory => gameHistory.stateHistory;
        List<SimObjectState>[] syncListOfEachTick;
        protected override void ExtraInit()
        {
            syncListOfEachTick = new List<SimObjectState>[tickHistory.Count];
            for (int i = 0; i < tickHistory.Count; i++)
            {
                syncListOfEachTick[i] = new List<SimObjectState>();
            }
            foreach (var state in stateHistory)
            {
                syncListOfEachTick[state.tick].Add(state);
            }
        }
        public void Run(GameHistory gameHistory)
        {
            this.gameHistory = gameHistory;
            Init(gameHistory.seed);
            StartCoroutine(RunSimulationCoroutine());
        }
        List<ISimObject> simListSnapshot;
        IEnumerator RunSimulationCoroutine()
        {
            int tick = 0;
            foreach (var tickCtx in tickHistory)
            {
                simListSnapshot = simRegistry.GetOrderedSimulationObjects().ToList();
                Tick(tick, tickCtx);
                yield return new WaitForFixedUpdate();
                tick++;
            }
        }
        void Tick(int tick, TickContext tickCtx)
        {
            tickCtx.rng = rng;
            foreach (var simObject in simListSnapshot)
            {
                simObject.Tick(tickCtx);
            }
            foreach (var state in syncListOfEachTick[tick])
            {
                int objectId = state.objectId;
                byte[] data = state.data;
                StateReader reader = new StateReader(data);
                ISimObject simObject = simRegistry.GetSimulationObject(objectId);
                simObject.DeserializeState(reader);
            }
            foreach (var simObject in simListSnapshot)
            {
                simObject.Render(tickCtx.deltaTime);
            }
            UnregisterQueuedObjects();
        }
    }
}
