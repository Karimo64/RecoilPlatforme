using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 3;

    protected int currentHealth;
    protected bool isDead;
    protected bool isActive; // Solo activo cuando está en cámara

    private Vector3 startPosition;
    private Quaternion startRotation;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        // Guardamos estado inicial (para respawn tipo Mega Man)
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Al inicio NO está activo
        isActive = false;
    }

    // Unity llama esto cuando entra en cámara (Game View)
    private void OnBecameVisible()
    {
        ActivateEnemy();
    }

    // Unity llama esto cuando sale de cámara
    private void OnBecameInvisible()
    {
        DeactivateEnemy();
    }

    protected virtual void ActivateEnemy()
    {
        ResetEnemy();
        isActive = true;
    }

    protected virtual void DeactivateEnemy()
    {
        isActive = false;
    }

    //  NUEVO: Detectar balas del jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si no puede actuar, ignoramos impactos
        if (!CanAct) return;

        // Si la bala es del jugador
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);                 // 1 hit por bala
            Destroy(collision.gameObject); // Destruir bala
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (!CanAct) return;

        currentHealth -= damage;
        OnDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        isActive = false;

        // No destruimos: se apaga hasta que salga y vuelva a entrar en cámara
        gameObject.SetActive(false);
    }

    public virtual void ResetEnemy()
    {
        isDead = false;
        currentHealth = maxHealth;

        // Reset físico
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Reactivar objeto
        gameObject.SetActive(true);
    }

    protected virtual void OnDamageFeedback()
    {
        Debug.Log($"{gameObject.name} took damage! HP: {currentHealth}");
    }

    // Contrato para los hijos
    protected bool CanAct => isActive && !isDead;
}
