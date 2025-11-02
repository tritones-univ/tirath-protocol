using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 moveDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezeRotationY;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
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

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

            Vector3 targetPos = rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }
}
