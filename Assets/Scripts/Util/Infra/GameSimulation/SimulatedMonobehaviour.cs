using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Simulation
{
    [TickOrder(TickOrder.DefaultOrder)]
    public abstract class SimulatedMonobehaviour : MonoBehaviour, ISimulationObject
    {
        virtual public void Init() {}
        virtual public void Tick(TickContext tickCtx) {}
        virtual public void SerializeState(StateWriter writer) {}
        virtual public void DeserializeState(StateReader reader) {}
    }
}