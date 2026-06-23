using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class DamageInfo
    {
        public float damage;
        public bool useHitEffect;
        public DamageInfo(float damage, bool useHitEffect = true)
        {
            this.damage = damage;
            this.useHitEffect = useHitEffect;
        }
    }
}