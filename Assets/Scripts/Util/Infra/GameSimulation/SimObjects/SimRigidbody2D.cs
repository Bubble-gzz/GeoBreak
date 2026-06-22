using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Util;
using Game.Render;

namespace Game.Simulation
{
    public class SimRigidbody2D : SimMonobehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private TransformRenderer transformRenderer;
        public Vector2 position { get => rb.position; set => rb.position = value; }
        public Vector2 velocity { get => rb.velocity; set => rb.velocity = value; }
        public float rotation { get => rb.rotation; set => rb.rotation = value; }
        override public void Init()
        {
            this.AutoFillComponentField(ref rb);
            velocity = Vector2.zero;
        }
        override public void Render(float deltaTime)
        {
            if (transformRenderer == null) {
                this.Log("TransformRenderer is not assigned", true);
                return;
            }
            transformRenderer.Render(new TransformData(position, rotation, new Vector2(1f, 1f)), deltaTime);
        }
        override public void SerializeState(StateWriter writer)
        {
            writer.WriteVector2(position);
            writer.WriteVector2(velocity);
        }
        override public void DeserializeState(StateReader reader)
        {
            position = reader.ReadVector2();
            velocity = reader.ReadVector2();
        }
        override public void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendVector2(sb, "position", position);
            StateSnapshotFormat.AppendVector2(sb, "velocity", velocity);
        }
    }

}
