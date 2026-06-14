using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Control
{
    /*
        提供物体移动能力的模块
    */
    public class MoveModule : MonoBehaviour
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
        void Awake()
        {
            if (rb == null) LogError("Rigidbody2D is not assigned");
            velocity = Vector2.zero;
        }

        void Update()
        {
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * velocity.magnitude * dampingWhenOverSpeed;
            }
            else {
                Vector2 dampVector = - velocity.normalized * Mathf.Min(damp * Time.deltaTime, velocity.magnitude);
                velocity += dampVector;
            }
            rb.velocity = velocity;
        }
        public void ApplyMoveDir(Vector2 dir)
        {
            if (dir.magnitude == 0) return;
            Vector2 v_accel = dir * accel * Time.deltaTime;

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
                        if (accel_along_v2 > 0) v2 = v2.normalized * Mathf.Max(v.magnitude, maxSpeed);
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
            velocity += dashDir * dashExtraSpeed;
        }
        private void LogError(string message)
        {
            Debug.LogError($"[{name}.MoveController] " + message);
        }
    }
}