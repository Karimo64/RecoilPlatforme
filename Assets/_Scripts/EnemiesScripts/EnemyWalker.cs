using UnityEngine;

public class EnemyWalker : EnemyBase
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private int direction = 1;

    private void Update()
    {
        if (isDead) return;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (!IsGroundAhead())
        {
            Flip();
        }
    }

    private bool IsGroundAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            0.2f,
            groundLayer
        );
        return hit.collider != null;
    }

    private void Flip()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}