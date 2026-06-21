using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

public class TestTrigger2D : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer spriteRenderer;
    Color originalColor;
    [SerializeField] Color triggerColor;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) this.LogError("No SpriteRenderer found", true);
        else {
            originalColor = spriteRenderer.color;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        this.Log("OnTriggerEnter2D: " + other.name, true);
        if (spriteRenderer != null) spriteRenderer.color = triggerColor;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        this.Log("OnTriggerExit2D: " + other.name, true);
        if (spriteRenderer != null) spriteRenderer.color = originalColor;
    }
}
