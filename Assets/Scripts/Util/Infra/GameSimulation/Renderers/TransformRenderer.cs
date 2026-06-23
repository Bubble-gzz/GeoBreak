using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Simulation;

namespace Game.Render
{
    public class TransformRenderer : MonoBehaviour
    {
        [SerializeField] private Transform renderTarget;
        private const float MinInterpolationDuration = 0.0001f;

        // 新增：可配置的最小差值
        [SerializeField] private float minDistanceThreshold = 0.01f;
        [SerializeField] private float minRotationThreshold = 0.5f;
        [SerializeField] private float minScaleThreshold = 0.01f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float startRotation;
        private float targetRotation;
        private Vector3 startScale;
        private Vector3 targetScale;
        private float interpolationDuration = MinInterpolationDuration;
        private float interpolationTime;
        private bool hasTarget;

        private void Awake()
        {
            if (renderTarget == null) renderTarget = transform;
        }

        private void Update()
        {
            if (!hasTarget) return;

            interpolationTime = Mathf.Min(interpolationTime + Time.deltaTime, interpolationDuration);
            float t = interpolationTime / interpolationDuration;

            // 计算距离、旋转、缩放的差值
            bool posClose = Vector3.Distance(startPosition, targetPosition) < minDistanceThreshold;
            bool rotClose = Mathf.Abs(Mathf.DeltaAngle(startRotation, targetRotation)) < minRotationThreshold;
            bool scaleClose = Vector3.Distance(startScale, targetScale) < minScaleThreshold;

            // 如果距离/旋转/缩放很近，直接“snapping”到目标，避免抖动
            renderTarget.position = posClose ? targetPosition : Vector3.Lerp(startPosition, targetPosition, t);
            renderTarget.rotation = rotClose
                ? Quaternion.Euler(0f, 0f, targetRotation)
                : Quaternion.Euler(0f, 0f, Mathf.LerpAngle(startRotation, targetRotation, t));
            renderTarget.localScale = scaleClose ? targetScale : Vector3.Lerp(startScale, targetScale, t);

            // 所有都到了目标，停止插值
            if (posClose && rotClose && scaleClose)
            {
                hasTarget = false;
            }
        }

        public void Render(TransformData data, float deltaTime)
        {
            if (renderTarget == null) renderTarget = transform;

            startPosition = renderTarget.position;
            targetPosition = new Vector3(data.position.x, data.position.y, renderTarget.position.z);
            startRotation = renderTarget.eulerAngles.z;
            targetRotation = data.rotation;
            startScale = renderTarget.localScale;
            targetScale = new Vector3(data.scale.x, data.scale.y, renderTarget.localScale.z);
            interpolationDuration = Mathf.Max(deltaTime, MinInterpolationDuration);
            interpolationTime = 0f;
            hasTarget = true;
        }
    }

    public class TransformData{
        public Vector2 position;
        public float rotation;
        public Vector2 scale;
        public TransformData(Vector2 position, float rotation, Vector2 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}