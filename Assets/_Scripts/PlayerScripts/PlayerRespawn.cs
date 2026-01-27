using UnityEngine;

// Controla el respawn del jugador
public class PlayerRespawn : MonoBehaviour, IFallable
{
    [Header("Respawn")]
    [SerializeField] private Transform currentCheckpoint;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Llamado por Deadzone (vía IFallable)
    public void OnFellIntoDeadzone()
    {
        Respawn();
    }

    private void Respawn()
    {
        // Reset físico
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Teleport seguro
        transform.position = currentCheckpoint.position;
    }

    // Llamado por checkpoints
    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }
}
