using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}
