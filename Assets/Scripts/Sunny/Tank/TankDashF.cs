using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankDashF : MonoBehaviour
{
    InputSystem inputActions;
    Rigidbody2D rb;

    TankMoveF tm;

    [SerializeField] GameObject tankBody;

    [SerializeField] float dashingPower = 16f;
    [SerializeField] float dashingTime = 4f;

    public bool canActiveDash { get; set; } = true;
    bool canDash = true;
    bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tm = GetComponent<TankMoveF>();

        inputActions = new InputSystem();
        inputActions.Enable();

        inputActions.Player1.Dash.started += OnDashP1;
        inputActions.Player2.Dash.started += OnDashP2;
    }

    void OnDashP1(InputAction.CallbackContext context)
    {
        if (canDash && canActiveDash)
        {
            StartCoroutine(Dash());
        }
    }

    void OnDashP2(InputAction.CallbackContext context)
    {
        if (canDash && canActiveDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {

        isDashing = true;
        canDash = false;

        rb.linearVelocity = tankBody.transform.right * dashingPower;

        yield return new WaitForSeconds(dashingTime);

        isDashing = false;
        canDash = true;
    }

    public bool IsDashing() => isDashing;
}
