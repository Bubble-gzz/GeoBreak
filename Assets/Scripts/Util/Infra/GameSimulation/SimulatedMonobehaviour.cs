using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Simulation
{
    [TickOrder(TickOrder.DefaultOrder)]
    public abstract class SimulatedMonobehaviour : MonoBehaviour, ISimulationObject
    {
        int simulationId;
        public int SimulationId => simulationId;

        public void SetSimulationId(int simulationId)
        {
            this.simulationId = simulationId;
        }

        virtual public void Init()
        {

        }
        virtual public void Tick(TickContext tickCtx)
        {

        }
    }
}