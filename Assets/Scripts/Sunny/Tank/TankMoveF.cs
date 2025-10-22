using UnityEngine;
using UnityEngine.InputSystem;

public class TankMoveF : MonoBehaviour
{
    InputSystem inputActions;
    Rigidbody2D rb;

    [SerializeField] GameObject tankBody;

    [SerializeField] float speed = 7f;
    [SerializeField] float acceleration = 4f;
    [SerializeField] float rotationSpeed = 250f; //10f

    enum PlayerNumber { Player1, Player2 };
    [SerializeField] PlayerNumber playerNumber;

    Vector2 moveInputP1;
    Vector2 moveInputP2;
    public Vector2 lastDirectionP1 = Vector2.right;
    public Vector2 lastDirectionP2 = Vector2.left;

    public bool canMove { get; set; } = true;
    bool isMovingP1;
    bool isMovingP2;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        inputActions = new InputSystem();
        inputActions.Enable();

        inputActions.Player1.Move.performed += OnMoveP1;
        inputActions.Player1.Move.canceled += OnMoveP1;
        inputActions.Player2.Move.performed += OnMoveP2;
        inputActions.Player2.Move.canceled += OnMoveP2;

        //if (playerNumber == PlayerNumber.Player1) transform.rotation = Quaternion.LookRotation(lastDirectionP1);
        //if (playerNumber == PlayerNumber.Player2) transform.rotation = Quaternion.LookRotation(lastDirectionP2);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (playerNumber == PlayerNumber.Player1)
            {
                Move(moveInputP1);
                lastDirectionP1 = RotateBody(moveInputP1, lastDirectionP1);
            }

            if (playerNumber == PlayerNumber.Player2)
            {
                Move(moveInputP2);
                lastDirectionP2 = RotateBody(moveInputP2, lastDirectionP2);
            }
        }
    }

    void OnMoveP1(InputAction.CallbackContext context)
    {
        
        moveInputP1 = context.ReadValue<Vector2>();

        if (context.performed) isMovingP1 = true;
        if (context.canceled) isMovingP1 = false;
    }

    void OnMoveP2(InputAction.CallbackContext context)
    {
        moveInputP2 = context.ReadValue<Vector2>();

        if (context.performed) isMovingP2 = true;
        if (context.canceled) isMovingP2 = false;
    }

    void Move(Vector2 moveInput)
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.Lerp(velocity.x, moveInput.x * speed, Time.fixedDeltaTime * acceleration);
        velocity.y = Mathf.Lerp(velocity.y, moveInput.y * speed, Time.fixedDeltaTime * acceleration);
        rb.linearVelocity = velocity;
    }

    Vector2 RotateBody(Vector2 moveInput, Vector2 lastDirection)
    {
        if (moveInput.sqrMagnitude > 0.01f)
            lastDirection = moveInput.normalized;

        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        tankBody.transform.rotation = Quaternion.Lerp(
            tankBody.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );

        return lastDirection;
    }

    public bool IsMovingP1() => isMovingP1;
    public bool IsMovingP2() => isMovingP2;

    public string GetPlayerNumber() => playerNumber == PlayerNumber.Player1 ? "Player1" : "Player2";

    public Vector2 GetLastDirection(int playerNum)
    {
        if (playerNum == 1) return lastDirectionP1;
        else return lastDirectionP2;
    }
}
