using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 direction; 

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Implement damage to player here
            Destroy(gameObject);
        }
    }
}