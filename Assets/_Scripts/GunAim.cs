using UnityEngine;
using UnityEngine.InputSystem;

public class GunAim : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Transform gunSprite;

    private Vector2 lastStickDir = Vector2.right;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector2 direction;

        // ─────────────────────────────────────────────
        // ¿Viene del stick? → tiene magnitud pequeña
        // ¿Viene del mouse? → es posición de pantalla
        // ─────────────────────────────────────────────

        if (input.sqrMagnitude <= 1.2f)   // ← probablemente joystick
        {
            // --- APUNTADO CON STICK DERECHO ---
            if (input.sqrMagnitude > 0.01f)
                lastStickDir = input;

            direction = lastStickDir;
        }
        else
        {
            // --- APUNTADO CON MOUSE ---
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(input);
            worldPos.z = 0f;
            direction = (worldPos - transform.position).normalized;
        }

        // Rotación real del arma
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Solo efecto visual del sprite
        Vector3 scale = gunSprite.localScale;
        scale.y = (direction.x < 0) ? -1 : 1;
        gunSprite.localScale = scale;
    }
}
