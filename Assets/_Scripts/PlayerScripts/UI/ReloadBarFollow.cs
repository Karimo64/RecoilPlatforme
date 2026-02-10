using UnityEngine;

public class ReloadBarFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.8f, 0f);

    void LateUpdate()
    {
        if (!player) return;
        transform.position = player.position + offset;
    }
}
