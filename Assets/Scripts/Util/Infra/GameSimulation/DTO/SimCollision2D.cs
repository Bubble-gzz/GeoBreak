using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Simulation
{
    public struct SimCollision2D
    {
        public GameObject other; // 外来碰撞体
        public SimCollision2D(GameObject other)
        {
            this.other = other;
        }
    }
}