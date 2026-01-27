using UnityEngine;

// Zona de eliminación
// Detecta TODO lo que entra
public class Deadzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Buscamos algo que pueda reaccionar a caídas
        IFallable fallable = other.GetComponent<IFallable>();

        if (fallable != null)
        {
            fallable.OnFellIntoDeadzone();
        }
    }
}
