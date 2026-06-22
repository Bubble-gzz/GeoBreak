using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Simulation;
using Game.Util;
using Game.Render;
using System.Text;

namespace Game.Core
{
    /*
        提供物体移动能力的模块
    */
    public class MoveModule : SimMonobehaviour
    {
        [SerializeField] public float maxSpeed = 10f;
        [SerializeField] public float dampTime = 0.1f;
        [SerializeField] public float dampingWhenOverSpeed = 0.95f;
        [SerializeField] public float accelTime = 0.1f;
        private float damp => maxSpeed / Mathf.Max(dampTime, 0.001f);
        private float accel => maxSpeed / Mathf.Max(accelTime, 0.001f) + damp;
        [SerializeField] public float dashExtraSpeed = 10f;
        [SerializeField] public SimRigidbody2D rb;
        private Vector2 velocity {get => rb.velocity; set => rb.velocity = value; }
        override public void Init()
        {
            AutoFillSimObjectField(ref rb);
        }
        public void UpdateVelocity(float deltaTime, bool skipCoastDamp)
        {
            if (velocity.magnitude > maxSpeed)
            {
                float dampFactor = Mathf.Pow(dampingWhenOverSpeed, deltaTime / GameConstants.referenceDeltaTime);
                velocity = velocity * dampFactor;
            }
            else {
                if (!skipCoastDamp) {
                    Vector2 dampVector = - velocity.normalized * Mathf.Min(damp * deltaTime, velocity.magnitude);
                    velocity = velocity + dampVector;
                }
            }
        }
        public void ApplyMoveDir(Vector2 dir, float deltaTime)
        {
            if (dir.magnitude == 0) return;
            Vector2 v_accel = dir * (accel - damp) * deltaTime;
            Vector2 v = velocity;
            Vector2 v2 = v + v_accel;
            if (v2.magnitude > maxSpeed)
            {
                if (v.magnitude == 0) v2 = v2.normalized * maxSpeed;
                else {
                    float v_along_v2 = Vector2.Dot(v, v2.normalized);
                    float accel_along_v2 = Vector2.Dot(v_accel, v2.normalized);
                    if (v_along_v2 > 0)
                    {
                        if (accel_along_v2 > 0)
                        {
                            v2 = v2.normalized * Mathf.Max(v.magnitude, maxSpeed);
                        }
                    }
                    else
                    {
                        v2 = v2.normalized * maxSpeed;
                    }

                }
            }
            velocity = v2;
        }
        public void ApplyDash(Vector2 dashDir)
        {
            if (dashDir.magnitude == 0) dashDir = velocity.normalized;
            velocity = velocity + dashDir * dashExtraSpeed;
        }
        override public void DescribeState(StringBuilder sb)
        {
            StateSnapshotFormat.AppendDescription(sb, "stateless module");
        }

    }
}