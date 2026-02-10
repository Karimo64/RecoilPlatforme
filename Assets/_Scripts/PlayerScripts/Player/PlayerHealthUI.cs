using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float current, float max)
    {
        float normalized = current / max;
        fillImage.fillAmount = normalized;
    }
}
