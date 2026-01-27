using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 0, -10f);

    void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = target.position + offset;
        pos.z = -10f;

        transform.position = pos;
    }
}
