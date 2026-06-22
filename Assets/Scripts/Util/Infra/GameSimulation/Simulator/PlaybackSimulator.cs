using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Simulation
{
    public class PlaybackSimulator : Simulator
    {
        private GameHistory gameHistory;
        List<TickContext> tickHistory => gameHistory.tickHistory;
        List<SimObjectState>[] syncListOfEachTick => gameHistory.syncListOfEachTick;
        protected override void ExtraInit()
        {
            tick = 0;
        }
        public void Run(GameHistory gameHistory)
        {
            this.gameHistory = gameHistory;
            Init(gameHistory.seed);
            StartCoroutine(RunSimulationCoroutine());
        }
        IEnumerator RunSimulationCoroutine()
        {
            tick = 0;
            foreach (var tickCtx in tickHistory)
            {
                Tick(BuildContext(tickCtx));
                yield return new WaitForFixedUpdate();
            }
            CloseLogFile();
            yield break;
        }
        TickContext BuildContext(TickContext tickCtx)
        {
            TickContext newTickCtx = tickCtx.Clone();
            newTickCtx.rng = rng;
            return newTickCtx;
        }
        void Tick(TickContext tickCtx)
        {
            BeforeTick();
            SimulateTick(tickCtx);
            ApplySyncStates(tick);
            RenderTick(tickCtx.deltaTime);
            EndTick();
            tick++;
        }
        void ApplySyncStates(int tick)
        {
            foreach (var state in syncListOfEachTick[tick])
            {
                StateReader reader = new StateReader(state.data);
                simRegistry.GetSimulationObject(state.objectId).DeserializeState(reader);
            }
        }
    }
}
