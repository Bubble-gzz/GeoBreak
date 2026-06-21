using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Render
{
    public class PositionRenderer : MonoBehaviour
    {
        [SerializeField] private Transform renderTarget;
        private const float MinInterpolationDuration = 0.0001f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
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
        }

        public void Render(Vector2 position, float deltaTime)
        {
            if (renderTarget == null) renderTarget = transform;

            startPosition = renderTarget.position;
            targetPosition = new Vector3(position.x, position.y, renderTarget.position.z);
            interpolationDuration = Mathf.Max(deltaTime, MinInterpolationDuration);
            interpolationTime = 0f;
            hasTarget = true;
        }
    }
}