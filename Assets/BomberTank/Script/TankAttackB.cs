using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttackB : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    // Update is called once per frame
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
