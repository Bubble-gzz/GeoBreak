using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Game.Util;

namespace Game.Simulation
{
    public class SimRigidbody2D : SimMonobehaviour
    {
        static SimRigidbody2D()
        {
            StateDescriberRegistry.Register<SimRigidbody2D>(DescribeSerializedState);
        }

        [SerializeField] private Rigidbody2D rb;
        public Vector2 position { get => rb.position; set => rb.position = value; }
        public Vector2 velocity { get => rb.velocity; set => rb.velocity = value; }
        public float rotation { get => rb.rotation; set => rb.rotation = value; }
        override public void Init()
        {
            this.AutoFillComponentField(ref rb);
            velocity = Vector2.zero;
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

        static void DescribeSerializedState(StateReader reader, StringBuilder sb)
        {
            StateSnapshotFormat.AppendVector2(sb, "position", reader.ReadVector2());
            StateSnapshotFormat.AppendVector2(sb, "velocity", reader.ReadVector2());
        }
    }

}
