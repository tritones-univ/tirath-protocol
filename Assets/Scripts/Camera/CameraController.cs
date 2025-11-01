using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    /* public float moveSpeed = 10f;
    public float height = 8f;
    public float angle = 35f;
    private Vector2 moveInput;
    private CameraInputAction inputActions;
    void Awake()
    {
        inputActions = new CameraInputAction();
    }
    void OnEnable()
    {
        inputActions.Camera.Enable();
        inputActions.Camera.Move.performed += OnMove;
        inputActions.Camera.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        inputActions.Camera.Move.performed -= OnMove;
        inputActions.Camera.Move.canceled -= OnMove;
        inputActions.Camera.Disable();
    }

    void Start()
    {
        transform.position = new Vector3(0, height, 0);
        transform.rotation = Quaternion.Euler(angle, 45f, 0f);
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void Update()
    {
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

        Vector3 move = (forward * moveInput.y + right * moveInput.x) * moveSpeed * Time.deltaTime;
        transform.position += move;
    } */

    public Transform target;
    public float followSpeed = 5f;
    public float height = 5f;
    public float angle = 35f;
    public Vector3 offset = new Vector3(-5f, 0, -5f);
    void Start()
    {
        transform.rotation = Quaternion.Euler(angle, 45f, 0f);
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = height;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}