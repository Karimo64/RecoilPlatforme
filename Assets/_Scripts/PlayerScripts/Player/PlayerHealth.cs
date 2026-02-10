using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public event Action<float, float> OnHealthChanged; // Current health, Max health
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    //Daño al jugador
    public void TakeDamage(float damage)
    {
        if (damage < 0) return; // No se permite daño negativo

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Curar al jugador
    public void Heal(float amount)
    {
        if (amount <= 0f) return; // No se permite curación negativa

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        // Aquí puedes agregar lógica adicional para manejar la muerte del jugador, como reproducir animaciones, desactivar controles, etc.
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
}
