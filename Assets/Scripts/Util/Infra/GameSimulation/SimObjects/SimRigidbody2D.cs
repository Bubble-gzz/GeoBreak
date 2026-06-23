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
        [SerializeField] private Rigidbody2DRenderer rigidbody2DRenderer;
        public Vector2 position { get => rb.position; set => rb.position = value; }
        public Vector2 velocity { get => rb.velocity; set => rb.velocity = value; }
        public float rotation { get => rb.rotation; set => rb.rotation = value; }
        public float angularVelocity { get => rb.angularVelocity; set => rb.angularVelocity = value; }
        override public void Init()
        {
            this.AutoFillComponentField(ref rb);
            velocity = Vector2.zero;
        }
        override public void Render(float deltaTime)
        {
            if (rigidbody2DRenderer == null) {
                this.Log("Rigidbody2DRenderer is not assigned", true);
                return;
            }
            rigidbody2DRenderer.Render(new Rigidbody2DData(position, rotation), deltaTime);
        }
        override public void SerializeState(StateWriter writer)
        {
            writer.WriteVector2(position);
            writer.WriteFloat(rotation);
            writer.WriteVector2(velocity);
            writer.WriteFloat(angularVelocity);
        }
        override public void DeserializeState(StateReader reader)
        {
            position = reader.ReadVector2();
            rotation = reader.ReadFloat();
            velocity = reader.ReadVector2();
            angularVelocity = reader.ReadFloat();
        }
        override public void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendVector2(sb, "position", position);
            StateSnapshotFormat.AppendFloat(sb, "rotation", rotation);
            StateSnapshotFormat.AppendVector2(sb, "velocity", velocity);
            StateSnapshotFormat.AppendFloat(sb, "angularVelocity", angularVelocity);
        }
    }

}
