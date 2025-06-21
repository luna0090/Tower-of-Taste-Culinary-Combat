using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;

    private Vector2 moveInput;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.ClimbModeGameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.ClimbModeGameplay.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.ClimbModeGameplay.Enable();
    }

    private void OnDisable()
    {
        controls.ClimbModeGameplay.Disable();
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move.sqrMagnitude > 0.001f)
        {
            // Move the character
            rb.MovePosition(rb.position + move.normalized * moveSpeed * Time.fixedDeltaTime);

            // Rotate toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.15f));
        }
    }
}
