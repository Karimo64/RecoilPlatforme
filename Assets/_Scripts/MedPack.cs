using UnityEngine;

/// <summary>
/// Restaura vida al jugador y desaparece tras un tiempo.
/// Incluye parpadeo progresivo antes de destruirse.
/// </summary>
public class MedPack : MonoBehaviour
{
    [Header("Heal Settings")]
    [SerializeField] private float healAmount = 20f;

    [Header("Lifetime Settings")]
    [SerializeField] private float lifetime = 15f;
    [SerializeField] private float firstBlinkStart = 10f;
    [SerializeField] private float fastBlinkStart = 13f;
    [SerializeField] private float slowBlinkInterval = 0.3f;
    [SerializeField] private float fastBlinkInterval = 0.1f;

    private SpriteRenderer spriteRenderer;

    private float lifeTimer = 0f;
    private float blinkTimer = 0f;
    private float currentBlinkInterval;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentBlinkInterval = slowBlinkInterval;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        // Destruir al finalizar el tiempo
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
            return;
        }

        // Determinar velocidad de parpadeo
        if (lifeTimer >= fastBlinkStart)
        {
            currentBlinkInterval = fastBlinkInterval;
        }
        else if (lifeTimer >= firstBlinkStart)
        {
            currentBlinkInterval = slowBlinkInterval;
        }
        else
        {
            return; // Aún no empieza a parpadear
        }

        HandleBlink();
    }

    private void HandleBlink()
    {
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= currentBlinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            blinkTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}