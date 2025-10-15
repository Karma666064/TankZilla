using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttackB : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private TankMoveB attack;
    private TankStateB state;

    private Vector3 powerUpBulletSize = new(5f, 2f, 1f);
    private Vector3 powerUpBulletSizeTwo = new(10f, 4f, 1f);
    private Vector3 powerUpBulletSizeThree = new(15f, 8f, 1f);

    private bool canAttack = true;
    private int numberUsedBullet = 0;

    [SerializeField] private Transform posBullet;

    void Start()
    {
        state = GetComponent<TankStateB>();
        attack = GetComponent<TankMoveB>();

        BulletMoveB.RetrieveAmmo += RetrieveAmmo;
    }

    // Update is called once per frame
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && canAttack)
        {
            numberUsedBullet += 1;

            if (numberUsedBullet >= state.numberAmmo)
                canAttack = false;

            GameObject tempo = Instantiate(bullet, posBullet.position, Quaternion.identity);
            BulletMoveB tempoMoveBullet = tempo.GetComponent<BulletMoveB>();

            tempoMoveBullet.id = state.id;
            tempoMoveBullet.power = state.power;
            tempoMoveBullet.isAoE = state.zoneAoE;

            switch (state.power)
            {
                case TankStateB.TankPowerBullet.levelOne:
                    break;
                case TankStateB.TankPowerBullet.levelTwo:
                    tempo.transform.localScale = powerUpBulletSize;
                    break;
                case TankStateB.TankPowerBullet.levelThree:
                    tempo.transform.localScale = powerUpBulletSizeTwo;
                    break;
                case TankStateB.TankPowerBullet.levelFour:
                    tempo.transform.localScale = powerUpBulletSizeThree;
                    break;
            }

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
    
    public void RetrieveAmmo(int id)
    {
        if (id == state.id)
        {
            numberUsedBullet -= 1;
            numberUsedBullet = Mathf.Clamp(numberUsedBullet, 0, state.numberAmmo);
            canAttack = true;
        }        
    }
}
