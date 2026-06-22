using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Simulation;
using Game.Util;
namespace Game.Core
{
    public class Bullet : SimMonobehaviour
    {
        [SerializeField] SimRigidbody2D rb;
        private Vector2 velocity {get => rb.velocity; set => rb.velocity = value; }
        private float rotation {get => rb.rotation; set => rb.rotation = value; }
        private float speed = 10f;
        private List<string> attackTargetTags = new List<string>();

        public override void Init()
        {
            AutoFillSimObjectField(ref rb);
        }
        public override void Tick(TickContext tickCtx)
        {
            velocity = transform.right * speed;
        }
        protected override void SimOnCollisionEnter2D(SimCollision2D collision)
        {
            GameObject other = collision.other;
            string otherTag = other.tag;
            string otherName = other.name;
            string otherLayer = other.layer.ToString();
            this.Log($"Bullet hit target: {otherTag} {otherName} {otherLayer}", true);
            SimDestory();
        }
        public override void SerializeState(StateWriter writer)
        {
            writer.WriteFloat(speed);
        }
        public override void DeserializeState(StateReader reader)
        {
            speed = reader.ReadFloat();
        }

        public override void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendFloat(sb, "speed", speed);
        }
    }
}

