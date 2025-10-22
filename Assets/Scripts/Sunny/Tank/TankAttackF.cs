using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttackF : MonoBehaviour
{
    InputSystem inputActions;

    TankMoveF tm;

    [SerializeField] GameObject tankTurret;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletSpawnPoint;

    [SerializeField] int damage = 1;
    [SerializeField] float rotationSpeed = 300f;
    [SerializeField] float bulletLifeTime = 4f;

    public int munitionMax = 3;
    public int currentMunitions;

    Vector2 lastJoystickDirection = Vector2.left;

    public bool canAttack { get; set; } = true;

    private void Awake()
    {
        tm = GetComponent<TankMoveF>();

        inputActions = new InputSystem();
        inputActions.Enable();

        inputActions.Player1.Attack.started += OnAttackP1;
        inputActions.Player2.Attack.started += OnAttackP2;
        inputActions.Player2.Target.performed += OnSetJoystick;
    }

    private void Start()
    {
        currentMunitions = munitionMax;
    }

    private void Update()
    {
        if (tm.GetPlayerNumber() == "Player1")
        {
            Vector2 direction = GetMouseDirection();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            tankTurret.transform.rotation = Quaternion.RotateTowards(
                tankTurret.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        if (tm.GetPlayerNumber() == "Player2")
        {
            Vector2 direction = lastJoystickDirection;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            tankTurret.transform.rotation = Quaternion.RotateTowards(
                tankTurret.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    void OnAttackP1(InputAction.CallbackContext context)
    {
        if (tm.GetPlayerNumber() == "Player1" && canAttack)
        {
            Attack(GetMouseDirection());
        }
    }

    void OnAttackP2(InputAction.CallbackContext context)
    {
        if (tm.GetPlayerNumber() == "Player2" && canAttack)
        {
            Attack(lastJoystickDirection);
        }
    }

    void OnSetJoystick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().sqrMagnitude > 0.1f)
            lastJoystickDirection = context.ReadValue<Vector2>();
    }

    void Attack(Vector2 direction)
    {
        if (currentMunitions > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletMakeDamageF>().damageToMake = damage;
            bullet.GetComponent<BulletMoveF>().lifeTime = bulletLifeTime;
            bullet.GetComponent<BulletMoveF>().direction = direction;

            currentMunitions--;
            if (currentMunitions < munitionMax)
            {
                StartCoroutine(Reloading(3f));
            }
        }
    }

    IEnumerator Reloading(float reloadingTime)
    {
        yield return new WaitForSeconds(reloadingTime);
        currentMunitions++;
    }

    Vector2 GetMouseDirection()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0f;

        return -(transform.position - mousePos).normalized;
    }
}
