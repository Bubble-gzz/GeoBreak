using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Util
{
    public interface IRandomGenerator
    {
        int NextInt(int min, int max);
        int seed { get; set; }
    }
    class RandomGenerator : IRandomGenerator
    {
        public int seed { get; set; }
        global::System.Random rng;
        public RandomGenerator(int seed)
        {
            this.seed = seed;
            rng = new global::System.Random(seed);
        }
        public int NextInt(int min, int max)
        {
            return rng.Next(min, max);
        }
    }
}