using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Render
{

    public class Rigidbody2DRenderer : MonoBehaviour
    {
        [SerializeField] private Transform renderTarget;
        private const float MinInterpolationDuration = 0.0001f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float startRotation;
        private float targetRotation;
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
        }

        public void Render(Rigidbody2DData data, float deltaTime)
        {
            if (renderTarget == null) renderTarget = transform;

            startPosition = renderTarget.position;
            targetPosition = new Vector3(data.position.x, data.position.y, renderTarget.position.z);
            startRotation = renderTarget.eulerAngles.z;
            targetRotation = data.rotation;
            interpolationDuration = Mathf.Max(deltaTime, MinInterpolationDuration);
            interpolationTime = 0f;
            hasTarget = true;
        }
    }
    public class Rigidbody2DData
    {
        public Vector2 position;
        public float rotation;
        public Rigidbody2DData(Vector2 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}