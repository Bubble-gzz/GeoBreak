using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Simulation
{
    public abstract class SimulatedMonobehaviour : MonoBehaviour, ISimulationObject
    {
        virtual public void Init()
        {

        }
        virtual public void Tick(TickCtx tickCtx)
        {

        }
    }
}