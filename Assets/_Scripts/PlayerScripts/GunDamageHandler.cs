using UnityEngine;

public class GunDamageHandler : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private float blinkInterval = 0.1f;

    private PlayerDamageHandler playerDamage;
    private SpriteRenderer[] gunSprites;

    private float blinkTimer;

    // Estado público para GunShoot
    public bool IsBlocked => playerDamage != null && playerDamage.IsInvulnerable;

    private void Awake()
    {
        // Tomamos el estado REAL del player
        playerDamage = GetComponentInParent<PlayerDamageHandler>();

        // Todos los sprites del arma (Gun, accesorios, etc.)
        gunSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerDamage == null) return;

        // Si el player NO está en invulnerabilidad → arma normal
        if (!playerDamage.IsInvulnerable)
        {
            SetSpritesVisible(true);
            blinkTimer = 0f;
            return;
        }

        // Blink sincronizado
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkInterval)
        {
            ToggleSprites();
            blinkTimer = 0f;
        }
    }

    private void ToggleSprites()
    {
        foreach (var sr in gunSprites)
        {
            sr.enabled = !sr.enabled;
        }
    }

    private void SetSpritesVisible(bool visible)
    {
        foreach (var sr in gunSprites)
        {
            sr.enabled = visible;
        }
    }
}
