using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TankMoveB : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField] private int speed = 400;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed * Time.fixedDeltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            direction = Vector2.zero;
        }
    }
}
