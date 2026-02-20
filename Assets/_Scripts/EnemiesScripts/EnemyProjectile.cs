using UnityEngine;

/// <summary>
/// Proyectil disparado por enemigos.
/// Se destruye automáticamente tras un tiempo definido.
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction;
    private float timer;

    /// <summary>
    /// Define la dirección del proyectil.
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        // Movimiento
        transform.Translate(direction * speed * Time.deltaTime);

        // Control de tiempo de vida
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Aquí puedes aplicar daño al jugador
            Destroy(gameObject);
        }
    }
}