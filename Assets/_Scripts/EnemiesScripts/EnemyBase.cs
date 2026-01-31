using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int maxHealth = 3;

    protected int currentHealth;
    protected bool isDead;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        OnDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void OnDamageFeedback()
    {
        // Implement visual or audio feedback for taking damage
        Debug.Log($"{gameObject.name} took damage! Current Health: {currentHealth}");
    }   

    protected virtual void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public virtual void ResetEnemy()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}
