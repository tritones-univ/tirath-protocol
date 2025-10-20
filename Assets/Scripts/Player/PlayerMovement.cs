using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        Vector3 moveDir = new Vector3(
            moveInput.x + moveInput.y,
            0,
            moveInput.y - moveInput.x
        ).normalized;

        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.1f);
    }
}
