using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Simulation;
using Game.Util;
using Game.Render;
namespace Game.Core
{
    public class Bullet : SimMonobehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] SimRigidbody2D rb;
        public DamageInfo damageInfo { get; set; }
        private Vector2 velocity {get => rb.velocity; set => rb.velocity = value; }
        private float rotation {get => rb.rotation; set => rb.rotation = value; }
        [SerializeField] private float speed = 10f;
        [SerializeField] private BulletRenderer bulletRenderer;
        [SerializeField] private float shapeReferenceSpeed = 5f;
        [SerializeField] private float speedDeformationFactor = 0.5f;
        [SerializeField] private float minLength = 0.5f;
        [SerializeField] private float maxLength = 2f;
        private List<string> attackTargetTags = new List<string>();
        private bool destroyed;

        public override void Init()
        {
            AutoFillSimObjectField(ref rb);
            //bulletRenderer.transform.SetParent(null);
            destroyed = false;
        }
        public void Setup(DamageInfo damageInfo, List<string> attackTargetTags)
        {
            this.damageInfo = damageInfo;
            this.attackTargetTags = attackTargetTags;
        }
        public override void Tick(TickContext tickCtx)
        {
            if (destroyed) {
                rb.velocity = Vector2.zero;
                return;
            }
            velocity = transform.right * speed;
            float length = (speed / shapeReferenceSpeed - 1) * speedDeformationFactor + 1;
            length = Mathf.Clamp(length, minLength, maxLength);
            transform.localScale = new Vector3(length, 1, 1);
        }
        protected override void SimOnCollisionEnter2D(SimCollision2D collision)
        {
            GameObject other = collision.other;
            string otherTag = other.tag;
            string otherName = other.name;
            string otherLayer = other.layer.ToString();
            if (attackTargetTags.Contains(otherTag))
            {
                other.GetComponentInChildren<IDamageable>()?.TakeDamage(damageInfo);
            }
            this.Log($"Bullet hit target: {otherTag} {otherName} {otherLayer}", true);
            rb.velocity = Vector2.zero;
            destroyed = true;
            SimDestory(root);
        }
        public override void Render(float deltaTime)
        {
            if (bulletRenderer == null) {
                this.Log("BulletRenderer is not assigned", true);
                return;
            }
            bulletRenderer.Render(new BulletRendererData(transform.localScale), deltaTime);
        }
        public override void SerializeState(StateWriter writer)
        {
            writer.WriteFloat(speed);
            writer.WriteBool(destroyed);
        }
        public override void DeserializeState(StateReader reader)
        {
            speed = reader.ReadFloat();
            destroyed = reader.ReadBool();
        }

        public override void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendFloat(sb, "speed", speed);
            StateSnapshotFormat.AppendBool(sb, "destroyed", destroyed);
        }
        
    }
}

