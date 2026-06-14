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
        [SerializeField] public float damp = 1f;
        [SerializeField] public float accel = 1f;
        [SerializeField] public float dashAccel = 1f;
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
            Vector2 dampVector = - velocity.normalized * Mathf.Min(damp * Time.deltaTime, velocity.magnitude);
            velocity += dampVector;
            rb.velocity = velocity;
        }
        public void ApplyMoveDir(Vector2 dir)
        {
            if (dir.magnitude == 0) return;
            Vector2 accelVector = dir * accel * Time.deltaTime;
            if (velocity.magnitude == 0) {
                velocity = accelVector;
                return;
            }
            // 将accelVector拆分成velocity方向的分量A和垂直方向B两部分
            Vector2 velocityDir = velocity.normalized;
            float accelAlongV = Vector2.Dot(accelVector, velocityDir);
            Vector2 accelVecAlongV = accelAlongV * velocityDir; // 方向A
            Vector2 accelVecPerpV = accelVector - accelVecAlongV; // 方向B
            accelVecAlongV = velocityDir * Mathf.Min(accelAlongV, Mathf.Max(0, maxSpeed - velocity.magnitude));
            velocity += accelVecAlongV + accelVecPerpV;
        }
        public void ApplyDash(Vector2 dashDir)
        {
            velocity += dashDir * dashAccel * Time.deltaTime;
        }
        private void LogError(string message)
        {
            Debug.LogError($"[{name}.MoveController] " + message);
        }
    }
}