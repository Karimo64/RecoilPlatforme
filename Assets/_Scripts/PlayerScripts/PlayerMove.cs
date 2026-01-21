using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Facing")]
    [SerializeField] float facingRightScale = 1f;
    [SerializeField] float facingLeftScale = -1f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        moveInput.x = Mathf.Clamp(moveInput.x, -1f, 1f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput.x > 0 ? facingRightScale : facingLeftScale;
            transform.localScale = scale;
        }
    }
}
