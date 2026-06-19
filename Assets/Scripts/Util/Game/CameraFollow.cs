using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Util
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime = 0.3f;
        private Vector3 velocity;
        [SerializeField] private Vector2 horizontalDeadZone = new Vector2(0.45f, 0.55f);
        [SerializeField] private Vector2 verticalDeadZone = new Vector2(0.45f, 0.55f);

        private Camera followCamera;

        private void Awake()
        {
            followCamera = GetComponent<Camera>();
            if (followCamera == null) LogError("Camera is not assigned");
            if (target == null) LogError("Target is not assigned");
        }
        void LogError(string message)
        {
            Debug.LogError($"[{name}.CameraFollow] " + message);
        }

        private void LateUpdate()
        {
            if (target == null || followCamera == null) return;

            Vector3 targetViewportPosition = followCamera.WorldToViewportPoint(target.position);
            Vector3 desiredPosition = transform.position;

            if (targetViewportPosition.x < horizontalDeadZone.x)
            {
                desiredPosition += ViewportDeltaToWorldDelta(
                    new Vector3(
                        targetViewportPosition.x - horizontalDeadZone.x,
                        0f,
                        targetViewportPosition.z
                    )
                );
            }
            else if (targetViewportPosition.x > horizontalDeadZone.y)
            {
                desiredPosition += ViewportDeltaToWorldDelta(
                    new Vector3(
                        targetViewportPosition.x - horizontalDeadZone.y,
                        0f,
                        targetViewportPosition.z
                    )
                );
            }

            if (targetViewportPosition.y < verticalDeadZone.x)
            {
                desiredPosition += ViewportDeltaToWorldDelta(
                    new Vector3(
                        0f,
                        targetViewportPosition.y - verticalDeadZone.x,
                        targetViewportPosition.z
                    )
                );
            }
            else if (targetViewportPosition.y > verticalDeadZone.y)
            {
                desiredPosition += ViewportDeltaToWorldDelta(
                    new Vector3(
                        0f,
                        targetViewportPosition.y - verticalDeadZone.y,
                        targetViewportPosition.z
                    )
                );
            }
            
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }

        private Vector3 ViewportDeltaToWorldDelta(Vector3 viewportDelta)
        {
            Vector3 origin = followCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, viewportDelta.z));
            Vector3 targetPosition = followCamera.ViewportToWorldPoint(new Vector3(0.5f + viewportDelta.x, 0.5f + viewportDelta.y, viewportDelta.z));
            Vector3 delta = targetPosition - origin;
            delta.z = 0f;
            return delta;
        }
    }
}