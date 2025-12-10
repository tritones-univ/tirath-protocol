using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Animaciones")]
    public Animator animator; // ← asigna el Animator aquí
    [SerializeField] private string walkAnimBool = "isWalk";

    private Rigidbody rb;
    // private Vector2 moveInput;
    private Vector2 moveInput = Vector2.zero; // <-- INICIALIZACIÓN EXPLÍCITA

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Si no se asignó manualmente el Animator, busca uno en hijos
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        moveInput = Vector2.zero; // <-- Doble chequeo de inicialización
    }

    /// <summary>
    /// Maneja la entrada de movimiento usando el nuevo Input System.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 1. Leer el valor de entrada mientras se está realizando la acción.
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        // 2. IMPORTANTE: Cuando la acción es cancelada (tecla liberada),
        //    establecer moveInput a cero para detener el movimiento.
        else if (context.canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // Nota: Asegúrate de que UIManager.Instance exista o quita esta línea si no es necesaria.
        // if (!UIManager.Instance.IsHUDOpen) return; 

        Debug.Log($"Valor de moveInput: {moveInput}");
        // Transforma el Vector2 de entrada (plano XZ) a un Vector3.
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);

        // Aplica una rotación de 45 grados (útil para juegos isométricos o 3/4).
        Quaternion rotation = Quaternion.Euler(0, 45f, 0);
        Vector3 moveDir = rotation * inputDir;

        // Comprueba si hay movimiento significativo.
        bool isMoving = moveDir.sqrMagnitude > 0.01f;

        // 🔹 Actualizar animación
        if (animator != null)
            animator.SetBool(walkAnimBool, isMoving);

        if (isMoving)
        {
            // Rotación suave: Mira en la dirección del movimiento
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

            // Movimiento físico: Aplica la velocidad en la dirección normalizada
            Vector3 targetPos = rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
        // Opcional: Si el personaje no se mueve, se puede forzar la velocidad a cero para evitar deslizamiento residual.
        // else
        // {
        //     rb.velocity = Vector3.zero;
        // }
    }
}