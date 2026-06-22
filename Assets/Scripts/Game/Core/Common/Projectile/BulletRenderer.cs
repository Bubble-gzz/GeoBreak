using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Render{
    public class BulletRenderer : MonoBehaviour
    {
        [SerializeField] private Transform renderTarget;
        private const float MinInterpolationDuration = 0.0001f;
        private float interpolationDuration = MinInterpolationDuration;
        private float interpolationTime;
        private bool hasTarget;
        private Vector3 startScale;
        private Vector3 targetScale;
        private void Awake()
        {
            if (renderTarget == null) renderTarget = transform;
        }
        private void Update()
        {
            if (!hasTarget) return;
            interpolationTime = Mathf.Min(interpolationTime + Time.deltaTime, interpolationDuration);
            float t = interpolationTime / interpolationDuration;
            renderTarget.localScale = Vector3.Lerp(startScale, targetScale, t);
        }
        public void Render(BulletRendererData data, float deltaTime)
        {
            if (renderTarget == null) renderTarget = transform;
            interpolationDuration = Mathf.Max(deltaTime, MinInterpolationDuration);
            interpolationTime = 0f;
            hasTarget = true;
            startScale = renderTarget.localScale;
            targetScale = data.scale;
        }
    }
    public class BulletRendererData{
        public Vector3 scale;
        public BulletRendererData(Vector3 scale){
            this.scale = scale;
        }
    }
}

