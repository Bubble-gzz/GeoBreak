using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Simulation;
namespace Game.Core.Control
{
    [TickOrder(TickOrder.RenderOrder)]
    public class AttackModule : SimulatedMonobehaviour
    {
        // Start is called before the first frame update
        Vector2 dir;
        override public void Init()
        {

        }
        override public void Tick(TickContext tickCtx)
        {
            
        }
        public void UpdateDir(Vector2 newDir)
        {
            dir = newDir;
        }
        public void Fire()
        {

        }
    }
}