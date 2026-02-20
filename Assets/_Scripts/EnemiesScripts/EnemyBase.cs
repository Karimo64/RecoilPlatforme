using UnityEngine;
using System.Collections;

/// <summary>
/// Clase base para todos los enemigos.
/// Maneja vida, muerte, respawn y drops.
/// </summary>
public class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int maxHealth = 1;
    protected int currentHealth;

    [Header("Respawn Settings")]
    [SerializeField] protected bool respawnable = true;
    [SerializeField] protected float respawnDelay = 15f;

    [Header("Respawn Protection")]
    [SerializeField] private float invulnerableTime = 1.75f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("Drop Settings")]
    [SerializeField] private DropData dropData;

    protected Vector3 initialPosition;
    protected bool isDead = false;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// Indica si el enemigo puede actuar (solo depende de si está vivo).
    /// </summary>
    protected bool CanAct => !isDead;

    protected virtual void Awake()
    {
        initialPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    #region Camera Check

    /// <summary>
    /// Verifica si el enemigo está dentro del viewport visible.
    /// Compatible con Cinemachine.
    /// </summary>
    protected bool IsInsideCamera()
    {
        if (Camera.main == null)
            return false;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        return viewportPos.x > 0f &&
               viewportPos.x < 1f &&
               viewportPos.y > 0f &&
               viewportPos.y < 1f &&
               viewportPos.z > 0f;
    }

    #endregion

    #region Damage System

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanAct) return;

        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (col != null)
            col.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        TryDropItem();

        if (respawnable)
            StartCoroutine(RespawnAfterDelay());
    }

    #endregion

    #region Drop System

    private void TryDropItem()
    {
        if (dropData == null || dropData.dropPrefab == null)
            return;

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= dropData.dropChancePercent)
        {
            Instantiate(dropData.dropPrefab, transform.position, Quaternion.identity);
        }
    }

    #endregion

    #region Respawn

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    protected virtual void Respawn()
    {
        transform.position = initialPosition;
        currentHealth = maxHealth;

        ResetState();
        StartCoroutine(RespawnProtection());
    }

    private IEnumerator RespawnProtection()
    {
        isDead = false;

        if (col != null)
            col.enabled = false;

        float timer = 0f;

        while (timer < invulnerableTime)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (col != null)
            col.enabled = true;
    }

    protected virtual void ResetState()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.Play("Idle");
    }

    #endregion
}