using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.System;
using Game.Simulation;

namespace Game.Core.Control
{
    /*
        提供物体移动能力的模块
    */
    [TickOrder(TickOrder.RenderOrder)]
    public class MoveModule : SimulatedMonobehaviour
    {
        [SerializeField] public float maxSpeed = 10f;
        [SerializeField] public float dampTime = 0.1f;
        [SerializeField] public float dampingWhenOverSpeed = 0.95f;
        [SerializeField] public float accelTime = 0.1f;
        private float damp => maxSpeed / Mathf.Max(dampTime, 0.001f);
        private float accel => maxSpeed / Mathf.Max(accelTime, 0.001f) + damp;
        [SerializeField] public float dashExtraSpeed = 10f;
        [SerializeField] public Rigidbody2D rb;
        [SerializeField] private Vector2 velocity;
        bool dampedThisTick;
        void Awake()
        {
            if (rb == null) LogError("Rigidbody2D is not assigned");
        }
        override public void Init()
        {
            velocity = Vector2.zero;
            dampedThisTick = false;
        }

        override public void Tick(TickContext tickCtx)
        {
            UpdateVelocity(tickCtx.deltaTime);
            dampedThisTick = false;
        }

        private void UpdateVelocity(float deltaTime)
        {
            if (velocity.magnitude > maxSpeed)
            {
                float dampFactor = Mathf.Pow(dampingWhenOverSpeed, deltaTime / GameConstants.referenceDeltaTime);
                velocity *= dampFactor;
            }
            else {
                if (!dampedThisTick) {
                    Vector2 dampVector = - velocity.normalized * Mathf.Min(damp * deltaTime, velocity.magnitude);
                    velocity += dampVector;
                    dampedThisTick = true;
                }
            }
            rb.velocity = velocity;
        }
        public void ApplyMoveDir(Vector2 dir, float deltaTime)
        {
            if (dir.magnitude == 0) return;
            Vector2 v_accel = dir * (accel - damp) * deltaTime;
            dampedThisTick = true;

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
            velocity += dashDir * dashExtraSpeed;
        }
        private void LogError(string msg)
        {
            Debug.LogError($"[{name}.MoveModule] " + msg);
        }
    }
}