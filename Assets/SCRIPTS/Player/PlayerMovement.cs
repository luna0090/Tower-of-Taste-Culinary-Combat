using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public Rigidbody rb;

    [Header("Jump / Roll / Dive Settings")]
    public float jumpForce = 5f;
    public float rollForce = 8f;
    public float diveForce = 10f;

    [Header("Sprint Stamina Settings")]
    public float maxStamina = 5f;                // Max stamina duration in seconds
    public float sprintDrainRate = 1f;           // Stamina drain rate while sprinting (per second)
    public float sprintRechargeRate = 2f;        // Stamina recharge rate (per second)
    public float sprintCooldownDuration = 2f;    // Cooldown after stamina is depleted

    private Vector2 moveInput;
    private bool isSprinting;
    private bool isJumpPressed;
    private bool isRollPressed;
    private bool isCtrlHeld;
    private bool isSpaceHeld;

    private float currentStamina;
    private float sprintCooldownTimer;
    private bool canSprint = true;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.ClimbModeGameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.ClimbModeGameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.ClimbModeGameplay.Sprint.performed += ctx => isSprinting = true;
        controls.ClimbModeGameplay.Sprint.canceled += ctx => isSprinting = false;

        controls.ClimbModeGameplay.Jump.performed += ctx =>
        {
            isJumpPressed = true;
            isSpaceHeld = true;
        };
        controls.ClimbModeGameplay.Jump.canceled += ctx => isSpaceHeld = false;

        controls.ClimbModeGameplay.Roll.performed += ctx =>
        {
            isRollPressed = true;
            isCtrlHeld = true;
        };
        controls.ClimbModeGameplay.Roll.canceled += ctx => isCtrlHeld = false;
    }

    private void OnEnable()
    {
        controls.ClimbModeGameplay.Enable();
    }

    private void OnDisable()
    {
        controls.ClimbModeGameplay.Disable();
    }

    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        bool wantsToSprint = isSprinting && moveInput.sqrMagnitude > 0.01f;

        HandleSprintStamina(wantsToSprint);

        float currentSpeed = wantsToSprint && canSprint ? moveSpeed * sprintMultiplier : moveSpeed;

        if (move.sqrMagnitude > 0.001f)
        {
            rb.MovePosition(rb.position + move.normalized * currentSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(move);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.15f));
        }

        if (isJumpPressed && IsGrounded())
        {
            if (isCtrlHeld)
                Dive(move);
            else
                Jump();
        }
        else if (isRollPressed && IsGrounded())
        {
            Roll(move);
        }

        // Reset one-time actions
        isJumpPressed = false;
        isRollPressed = false;
    }

    private void HandleSprintStamina(bool sprinting)
    {
        if (sprinting && canSprint)
        {
            currentStamina -= sprintDrainRate * Time.fixedDeltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false;
                sprintCooldownTimer = sprintCooldownDuration;
            }
        }
        else
        {
            if (sprintCooldownTimer > 0f)
            {
                sprintCooldownTimer -= Time.fixedDeltaTime;
            }
            else
            {
                if (currentStamina < maxStamina)
                {
                    currentStamina += sprintRechargeRate * Time.fixedDeltaTime;
                    if (currentStamina >= maxStamina)
                    {
                        currentStamina = maxStamina;
                        canSprint = true;
                    }
                }
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Roll(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
            rb.AddForce(direction.normalized * rollForce, ForceMode.Impulse);
    }

    private void Dive(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            Vector3 diveVector = direction.normalized * diveForce + Vector3.up * (jumpForce / 2f);
            rb.AddForce(diveVector, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
