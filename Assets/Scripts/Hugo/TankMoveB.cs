using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TankMoveB : MonoBehaviour
{
    private Vector2 saveVector;
    private InputSystem inputActions;
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public Direction currentDirection;
    private Vector2 moveInput;

    private TankStateB state;
    [SerializeField] private int speed = 400;
    private Rigidbody2D rb;
    [SerializeField] private int id = 1;
    void Start()
    {
        state = GetComponent<TankStateB>();
        currentDirection = Direction.Right;
        rb = GetComponent<Rigidbody2D>();

        inputActions = new InputSystem();
        inputActions.Enable();

        if (id == 1)
        {
            inputActions.Player1.Move.performed += OnMove;
            inputActions.Player1.Move.canceled += OnMove;
        }
        else if (id == 2)
        {
            inputActions.Player2.Move.started += OnMove;
            inputActions.Player2.Move.performed += OnMove;
            inputActions.Player2.Move.canceled += OnMove;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed * Time.fixedDeltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!state.endgame)
        {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput.y < -0.3)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                currentDirection = Direction.Down;
            }
            else if (moveInput.y > 0.3)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                currentDirection = Direction.Up;
            }
            else if (moveInput.x < -0.3)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                currentDirection = Direction.Left;
            }
            else if (moveInput.x > 0.3)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                currentDirection = Direction.Right;
            }

            if (context.canceled)
            {
                saveVector = moveInput;
                moveInput = Vector2.zero;
            }
        }
    }
}
