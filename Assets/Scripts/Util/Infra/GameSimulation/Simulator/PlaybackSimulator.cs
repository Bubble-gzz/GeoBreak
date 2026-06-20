using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        IEnumerator RunSimulationCoroutine()
        {
            int tick = 0;
            foreach (var tickCtx in tickHistory)
            {
                Tick(tick, tickCtx);
                yield return new WaitForFixedUpdate();
                tick++;
            }
        }
        void Tick(int tick, TickContext tickCtx)
        {
            foreach (var simObject in simRegistry.GetOrderedSimulationObjects())
            {
                simObject.Tick(tickCtx);
            }
            foreach (var state in syncListOfEachTick[tick])
            {
                string objectId = state.objectId;
                byte[] data = state.data;
                StateReader reader = new StateReader(data);
                ISimulationObject simObject = simRegistry.GetSimulationObject(objectId);
                simObject.DeserializeState(reader);
            }
            foreach (var simObject in simRegistry.GetOrderedSimulationObjects())
            {
                simObject.Render(tickCtx.deltaTime);
            }
        }
    }
    public class GameHistory{
        public int seed {get; private set;}
        public List<TickContext> tickHistory {get; private set;}
        public List<SimObjectState> stateHistory {get; private set;}
        public GameHistory(int seed, List<TickContext> tickHistory, List<SimObjectState> stateHistory)
        {
            this.seed = seed;
            this.tickHistory = tickHistory;
            this.stateHistory = stateHistory;
        }
    }
}
