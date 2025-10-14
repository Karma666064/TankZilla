using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttackB : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private TankMoveB attack;
    private TankStateB state;

    [SerializeField] private Transform posBullet;

    void Start()
    {
        state = GetComponent<TankStateB>();
        attack = GetComponent<TankMoveB>();
    }

    // Update is called once per frame
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameObject tempo = Instantiate(bullet, posBullet.position, Quaternion.identity);
            BulletMoveB tempoMoveBullet = tempo.GetComponent<BulletMoveB>();

            tempoMoveBullet.id = state.id;
            tempo.transform.localScale = new Vector3(5f, 2f, 1f);
            
            switch (attack.currentDirection)
            {
                case TankMoveB.Direction.Up:
                    tempoMoveBullet.directionBullet = Vector3.up;
                    break;
                case TankMoveB.Direction.Down:
                    tempoMoveBullet.directionBullet = Vector3.down;
                    tempo.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case TankMoveB.Direction.Left:
                    tempoMoveBullet.directionBullet = Vector3.left;
                    tempo.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                case TankMoveB.Direction.Right:
                    tempoMoveBullet.directionBullet = Vector3.right;
                    tempo.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                default:
                    break;
            }
            
        }
    }
}
