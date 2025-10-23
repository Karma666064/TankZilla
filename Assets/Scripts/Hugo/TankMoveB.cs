using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TankMoveB : MonoBehaviour
{
    private Vector2 saveVector;
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public Direction currentDirection;
    private Vector2 moveInput;
    [SerializeField] private int speed = 400;
    private Rigidbody2D rb;
    void Start()
    {
        currentDirection = Direction.Right;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed * Time.fixedDeltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove");
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
