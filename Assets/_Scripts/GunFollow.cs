using UnityEngine;

public class GunFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    void LateUpdate()
    {
        transform.position = player.position;
    }
}
