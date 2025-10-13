using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttackF : MonoBehaviour
{
    InputSystemFootTank inputActions;

    TankMoveF tm;

    [SerializeField] GameObject tankTurret;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bulletSpawnPoint;

    [SerializeField] int damage = 1;
    [SerializeField] float rotationSpeed = 300f;
    [SerializeField] float bulletLifeTime = 4f;
    [SerializeField] int munitionMax = 3;

    int currentMunitions;

    private void Awake()
    {
        tm = GetComponent<TankMoveF>();

        inputActions = new InputSystemFootTank();
        inputActions.Enable();

        inputActions.Player1.Attack.started += OnAttack;
    }

    private void Start()
    {
        currentMunitions = munitionMax;
    }

    private void Update()
    {
        Vector2 direction = GetMousePos();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        tankTurret.transform.rotation = Quaternion.RotateTowards(
            tankTurret.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (currentMunitions > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletMakeDamageF>().damageToMake = damage;
            bullet.GetComponent<BulletMoveF>().lifeTime = bulletLifeTime;
            bullet.GetComponent<BulletMoveF>().direction = GetMousePos();

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

    Vector2 GetMousePos()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0f;

        return mousePos;
    }
}
