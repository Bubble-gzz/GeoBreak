using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

namespace Game.Simulation
{
    public class SimRigidbody : SimulatedMonobehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        public override void Init()
        {
            if (rb == null) {
                rb = GetComponent<Rigidbody2D>();
                if (rb != null) this.Log("Auto assigned Rigidbody2D", true);
                else this.LogError("No Rigidbody2D found", true);
            }
        }
        public override void SerializeState(StateWriter writer)
        {
            writer.WriteVector2(rb.velocity);
            writer.WriteVector2(rb.position);
            writer.WriteFloat(rb.rotation);
            writer.WriteFloat(rb.angularVelocity);
        }
        public override void DeserializeState(StateReader reader)
        {
            rb.velocity = reader.ReadVector2();
            rb.position = reader.ReadVector2();
            rb.rotation = reader.ReadFloat();
            rb.angularVelocity = reader.ReadFloat();
        }
        public Vector2 velocity {get => rb.velocity; set => rb.velocity = value;}
        public Vector2 position {get => rb.position; set => rb.position = value;}
        public float rotation {get => rb.rotation; set => rb.rotation = value;}
        public float angularVelocity {get => rb.angularVelocity; set => rb.angularVelocity = value;}
    }
}