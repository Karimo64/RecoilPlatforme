using UnityEngine;

public class MedPack : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f;


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
