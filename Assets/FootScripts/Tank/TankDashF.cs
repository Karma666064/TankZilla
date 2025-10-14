using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankDashF : MonoBehaviour
{
    InputSystemFootTank inputActions;
    Rigidbody2D rb;

    TankMoveF tm;

    [SerializeField] float dashingPower = 16f;
    [SerializeField] float dashingTime = 4f;

    bool canDash = true;
    bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tm = GetComponent<TankMoveF>();

        inputActions = new InputSystemFootTank();
        inputActions.Enable();

        inputActions.Player1.Dash.started += OnDash;
        inputActions.Player2.Dash.started += OnDash;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        rb.linearVelocity = transform.forward * dashingPower;

        yield return new WaitForSeconds(dashingTime);

        isDashing = false;
        canDash = true;
    }

    public bool IsDashing() => isDashing;
}
