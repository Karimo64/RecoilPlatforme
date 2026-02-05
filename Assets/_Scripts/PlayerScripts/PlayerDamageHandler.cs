using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float invulnerabilityTime = 1f;

    [Header("Visual")]
    [SerializeField] private float blinkInterval = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Estado interno
    private bool isInvulnerable;
    private float blinkTimer;

    // EXPONEMOS el estado para que otros scripts (PlayerMove)
    // puedan saber si el jugador está en knockback
    public bool IsInvulnerable => isInvulnerable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Si no está invulnerable, no parpadea
        if (!isInvulnerable) return;

        // Temporizador del blink
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkInterval)
        {
            // Encendemos / apagamos el sprite
            spriteRenderer.enabled = !spriteRenderer.enabled;
            blinkTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si ya está invulnerable, ignoramos golpes
        if (isInvulnerable) return;

        // Daño genérico (balas, trampas, etc.)
        if (collision.CompareTag("Damage"))
        {
            ApplyKnockback();
            Debug.Log("Daño detectado, push back");

            Destroy(collision.gameObject);
        }

        // Enemigo cuerpo a cuerpo
        if (collision.CompareTag("Enemy"))
        {
            ApplyKnockback();
            Debug.Log("Daño detectado, push back");
        }
    }

    private void ApplyKnockback()
    {
        // Activamos invulnerabilidad
        isInvulnerable = true;

        // Detectamos hacia dónde mira el jugador
        // localScale.x > 0 → mira derecha
        // localScale.x < 0 → mira izquierda
        float facingDirection = Mathf.Sign(transform.localScale.x);

        // Empujón SIEMPRE hacia atrás (horizontal puro)
        Vector2 knockbackDirection = new Vector2(-facingDirection, 0f);

        // Cancelamos solo la velocidad horizontal previa
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Aplicamos el empujón
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // Programamos el fin de la invulnerabilidad
        Invoke(nameof(ResetInvulnerability), invulnerabilityTime);
    }

    private void ResetInvulnerability()
    {
        // Terminó el daño
        isInvulnerable = false;

        // Reiniciamos blink
        blinkTimer = 0f;

        // MUY IMPORTANTE: el sprite vuelve visible
        spriteRenderer.enabled = true;
    }
}
