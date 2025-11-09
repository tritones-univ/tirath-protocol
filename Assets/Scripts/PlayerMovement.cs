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
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Si no se asignó manualmente el Animator, busca uno en hijos
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (!UIManager.Instance.IsHUDOpen) return;

        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y);
        Quaternion rotation = Quaternion.Euler(0, 45f, 0);
        Vector3 moveDir = rotation * inputDir;

        bool isMoving = moveDir.sqrMagnitude > 0.01f;

        // 🔹 Actualizar animación
        if (animator != null)
            animator.SetBool(walkAnimBool, isMoving);

        if (isMoving)
        {
            // Rotación suave
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

            // Movimiento físico
            Vector3 targetPos = rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }
}
