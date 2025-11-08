using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Eventos de interacción")]
    public UnityEvent onInteract; // Puedes suscribirte desde el editor o por código

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onInteract?.Invoke();
        }
    }
}
