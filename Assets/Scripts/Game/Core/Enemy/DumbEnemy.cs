using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Simulation;
using System.Text;
using Game.Util;
using Game.Render;

namespace Game.Core
{
    public class DumbEnemy : SimMonobehaviour, IDamageable
    {
        [SerializeField] private HitEffectRenderer hitEffectRenderer;
        [SerializeField] private float maxHp = 3;
        private float hp;
        override public void Init()
        {
            hp = maxHp;
        }
        List<DamageInfo> pendingDamageInfos = new List<DamageInfo>();
        public void TakeDamage(DamageInfo damageInfo)
        {
            this.Log($"Take damage: {damageInfo.damage}", true);
            pendingDamageInfos.Add(damageInfo);
            hp = Mathf.Max(0f, hp - damageInfo.damage);
            if (hp <= 0f)
            {
                Die();
            }
        }
        public void Die()
        {
            Destroy(gameObject);
        }
        override public void Render(float deltaTime)
        {
            bool useHitEffect = false;
            foreach (var damageInfo in pendingDamageInfos)
                useHitEffect |= damageInfo.useHitEffect;
            if (useHitEffect)
            {
                hitEffectRenderer.Render(deltaTime);
            }
            pendingDamageInfos.Clear();
        }
        public override void SerializeState(StateWriter writer)
        {
            writer.WriteFloat(hp);
        }
        public override void DeserializeState(StateReader reader)
        {
            hp = reader.ReadFloat();
        }
        public override void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendFloat(sb, "hp", hp);
        }
    }
}