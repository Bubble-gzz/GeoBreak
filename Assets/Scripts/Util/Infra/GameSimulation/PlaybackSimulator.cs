using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Simulation
{
    public class PlaybackSimulator : Simulator
    {
        private GameHistory gameHistory;
        List<TickContext> tickHistory => gameHistory.tickHistory;
        public void Run(GameHistory gameHistory)
        {
            this.gameHistory = gameHistory;
            Init(gameHistory.seed);
            StartCoroutine(RunSimulationCoroutine());
        }
        IEnumerator RunSimulationCoroutine()
        {
            foreach (var tickCtx in tickHistory)
            {
                Tick(tickCtx);
                yield return new WaitForFixedUpdate();
            }
        }
        void Tick(TickContext tickCtx)
        {
            foreach (var simulationObject in simRegistry.GetOrderedSimulationObjects())
            {
                simulationObject.Tick(tickCtx);
            }
        }
    }
    public class GameHistory{
        public int seed {get; private set;}
        public List<TickContext> tickHistory {get; private set;}
        public GameHistory(int seed, List<TickContext> tickHistory)
        {
            this.seed = seed;
            this.tickHistory = tickHistory;
        }
    }
}
