using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Simulation
{
    public class TransformRenderer : MonoBehaviour, IRenderObject<TransformData>
    {
        [SerializeField] private Transform renderTarget;
        private const float MinInterpolationDuration = 0.0001f;

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

            renderTarget.position = Vector3.Lerp(startPosition, targetPosition, t);
            renderTarget.rotation = Quaternion.Euler(0f, 0f, Mathf.LerpAngle(startRotation, targetRotation, t));
            renderTarget.localScale = Vector3.Lerp(startScale, targetScale, t);
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
    }
}