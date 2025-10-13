using UnityEngine;
using UnityEngine.InputSystem;

public class TankMoveF : MonoBehaviour
{
    InputSystemFootTank inputActions;
    Rigidbody2D rb;

    [SerializeField] GameObject tankBody;

    [SerializeField] float speed = 7f;
    [SerializeField] float acceleration = 4f;
    [SerializeField] float rotationSpeed = 250f; //10f

    Vector2 moveInput;
    Vector2 lastDirection = Vector2.right;

    bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        inputActions = new InputSystemFootTank();
        inputActions.Enable();

        inputActions.Player1.Move.performed += OnMove;
        inputActions.Player1.Move.canceled += OnMove;
    }

    private void FixedUpdate()
    {
        // Déplacement
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * speed, Time.fixedDeltaTime * acceleration);
        velocity.y = Mathf.Lerp(velocity.y, moveInput.y * speed, Time.fixedDeltaTime * acceleration);
        rb.linearVelocity = velocity;

        // Modifier la rotation du body
        if (moveInput.sqrMagnitude > 0.001f)
            lastDirection = moveInput;

        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        tankBody.transform.rotation = Quaternion.RotateTowards(
            tankBody.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.performed) isMoving = true;
        if (context.canceled) isMoving = false;
    }

    public bool IsMoving() => isMoving;

    public Vector2 GetLastDirection() => lastDirection;
}
