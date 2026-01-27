using UnityEngine;

// Checkpoint de nivel (no persistente)
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            respawn.SetCheckpoint(transform);
        }
    }
}
