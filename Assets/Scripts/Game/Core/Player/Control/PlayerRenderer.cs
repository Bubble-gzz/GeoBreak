using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Render
{
    public class PlayerRenderer : MonoBehaviour
    {
        Transform simObjectTransform;
        [SerializeField] private Transform positionTarget;
        [SerializeField] private Transform rotationTarget;

        float startAngle;
        float targetAngle;
        float interpolationDuration;
        float interpolationTime;

        void Update()
        {
            if (simObjectTransform == null) return;

            // 位置可以直接赋值，也可以做平滑移动
            if (positionTarget != null && simObjectTransform != null)
            {
                positionTarget.position = simObjectTransform.position; 
            }

            if (rotationTarget != null)
            {
                interpolationTime = Mathf.Min(interpolationTime + Time.deltaTime, interpolationDuration);
                float t = interpolationTime / interpolationDuration;
                float newAngle = Mathf.LerpAngle(startAngle, targetAngle, t);
                rotationTarget.rotation = Quaternion.Euler(0f, 0f, newAngle);
            }
        }

        public void Render(PlayerRendererData data, float deltaTime)
        {
            simObjectTransform = data.simObjectTransform;
            targetAngle = data.aimAngle;
            startAngle = rotationTarget.eulerAngles.z;
            interpolationDuration = Mathf.Max(deltaTime, 0.0001f);
            interpolationTime = 0f;
            // smoothTime和maxSpeed可以在Inspector上调节，或者根据deltaTime适当微调
            // 若需更快响应，在开火或快速切换可考虑重置currentVelocity = 0;
        }
    }
    public class PlayerRendererData{
        public Transform simObjectTransform;
        public float aimAngle;
        public Vector2 scale;
        public PlayerRendererData(Transform simObjectTransform, float aimAngle, Vector2 scale)
        {
            this.simObjectTransform = simObjectTransform;
            this.aimAngle = aimAngle;
            this.scale = scale;
        }
    }
}