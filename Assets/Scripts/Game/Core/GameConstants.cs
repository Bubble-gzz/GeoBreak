using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Core
{
    public static class GameConstants
    {
        public static float referenceDeltaTime = 0.02f; //模拟计算的基准，用于消除帧率影响
        public static string PlayerProjectileLayerName = "PlayerProjectile";
    }
}
