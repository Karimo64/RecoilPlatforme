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

    // NUEVO: referencia al daño
    private PlayerDamageHandler damageHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Buscamos el PlayerDamageHandler en el mismo GameObject
        damageHandler = GetComponent<PlayerDamageHandler>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        moveInput.x = Mathf.Clamp(moveInput.x, -1f, 1f);
    }

    private void FixedUpdate()
    {
        // SI el jugador está en knockback / invulnerable
        // NO permitimos movimiento por input
        if (damageHandler != null && damageHandler.IsInvulnerable)
            return;

        // Movimiento normal
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // Volteo visual
        if (moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput.x > 0 ? facingRightScale : facingLeftScale;
            transform.localScale = scale;
        }
    }
}
