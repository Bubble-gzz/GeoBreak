using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Util;

namespace Game.Simulation
{
    public class TickContext{
        public float deltaTime;
        public IRandomGenerator rng;
        public List<InputData> inputDatas = new List<InputData>();
        public List<GameSettings> gameSettings = new List<GameSettings>();
        override public string ToString()
        {
            return $"{{ deltaTime: {deltaTime} }}";
        }
    }
}