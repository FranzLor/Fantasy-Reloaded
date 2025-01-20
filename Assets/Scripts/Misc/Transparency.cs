using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Transparency : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float transparencyAmount = 0.75f;
    [SerializeField] private float fadeTime = 0.5f;

    // used for parented objs too
    private SpriteRenderer[] spriteRenderers;
    private Tilemap[] tilemaps;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        tilemaps = GetComponentsInChildren<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderers != null && spriteRenderers.Length > 0)
            {
                foreach (var spriteRenderer in spriteRenderers)
                {
                    if (spriteRenderer != null)
                    {
                        StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
                    }
                }
            }

            if (tilemaps != null && tilemaps.Length > 0)
            {
                foreach (var tilemap in tilemaps)
                {
                    if (tilemap != null) // Ensure each Tilemap is valid
                    {
                        StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
                    }
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderers != null && spriteRenderers.Length > 0)
            {
                foreach (var spriteRenderer in spriteRenderers)
                {
                    if (spriteRenderer != null)
                    {
                        StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1.0f));
                    }
                }
            }

            if (tilemaps != null && tilemaps.Length > 0)
            {
                foreach (var tilemap in tilemaps)
                {
                    if (tilemap != null)
                    {
                        StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1.0f));
                    }
                }
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            // must change sprite color to change alpha value
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            // must change sprite color to change alpha value
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }

}
