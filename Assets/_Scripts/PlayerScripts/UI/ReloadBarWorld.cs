using UnityEngine;

public class ReloadBarWorld : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunShoot gun;
    [SerializeField] private Transform fill;

    [Header("Fade")]
    [SerializeField] private float fadeSpeed = 8f;

    private SpriteRenderer[] renderers;
    private float targetAlpha = 0f;

    private Vector3 fillBaseScale; // <- escala correcta (0.55, 0.05, 1)

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        fillBaseScale = fill.localScale; // guardamos escala real
        SetAlpha(0f);
    }

    private void Update()
    {
        if (gun.IsReloading())
        {
            targetAlpha = 1f;

            float progress = gun.GetReloadProgress();

            // Escalar SOLO X manteniendo tamaño real
            fill.localScale = new Vector3(
                fillBaseScale.x * progress,
                fillBaseScale.y,
                fillBaseScale.z
            );
        }
        else
        {
            targetAlpha = 0f;
        }

        float currentAlpha = renderers[0].color.a;
        float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
        SetAlpha(newAlpha);
    }

    private void SetAlpha(float a)
    {
        foreach (var r in renderers)
        {
            Color c = r.color;
            c.a = a;
            r.color = c;
        }
    }
}
