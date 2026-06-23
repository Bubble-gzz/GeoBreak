using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

public class HitEffectRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float highlight = 1.5f; // 亮度增强倍数
    [SerializeField] private float decayPercent = 0.85f; // 每帧颜色亮度衰减比例
    private Color baseColor;

    [SerializeField] private float waitTimer = 0f;
    [SerializeField] private bool isRendering = false;
    [SerializeField] private Color targetColor;
    private void Awake()
    {
        if (spriteRenderer == null) {
            this.AutoFillComponentField(ref spriteRenderer, autoAdd: false, searchInChildren: true);
            if (spriteRenderer != null) this.Log("Auto assigned SpriteRenderer", true);
            else this.LogError("No SpriteRenderer found", true);
        }
        baseColor = spriteRenderer.color;
    }
    public void Render(float deltaTime)
    {
        // render被调用, 等待deltaTime秒后开始渲染效果
        this.Log($"Render hit effect, deltaTime: {deltaTime}", true);
        waitTimer = deltaTime;
        isRendering = false;
    }

    private void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                // 开始渲染击中效果, 增强亮度
                isRendering = true;
                targetColor = baseColor * highlight;
                targetColor.a = baseColor.a; // 防止alpha乘高
                if (spriteRenderer != null)
                    spriteRenderer.color = targetColor;
            }
        }
        else if (isRendering)
        {
            if (spriteRenderer != null)
            {
                // 指数规律衰减到baseColor
                targetColor = Color.Lerp(baseColor, spriteRenderer.color, decayPercent);
                spriteRenderer.color = targetColor;

                // 当亮度接近正常时, 结束效果
                if (Vector4.Distance(spriteRenderer.color, baseColor) < 0.01f)
                {
                    spriteRenderer.color = baseColor;
                    isRendering = false;
                }
            }
        }
    }
}
