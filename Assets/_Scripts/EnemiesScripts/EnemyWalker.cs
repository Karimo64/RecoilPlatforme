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
        if (!CanAct)
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        Move();
    }

    private void Move()
    {
        if (rb != null)
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (!IsGroundAhead())
        {
            Flip();
        }
    }

    private bool IsGroundAhead()
    {
        if (groundCheck == null) return false;

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

    protected override void ResetState()
    {
        base.ResetState();

        direction = 1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}