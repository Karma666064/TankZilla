using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BulletMoveB : MonoBehaviour
{
    public static event Action<int> RetrieveAmmo;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speed = 100;

    public ExplosionPoolManager explosionPool;
    public Vector3 directionBullet;
    public int id;
    public TankStateB.TankPowerBullet power;
    public int sizeAoE;
    private bool isExplosing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TimerDestroy());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * directionBullet;
    }

    IEnumerator TimerDestroy()
    {
        yield return new WaitForSeconds(3f);
        DestroyOnContact(3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            if (collision.GetComponent<TankStateB>().id != id)
            {
                TankLifeB tankLife = collision.GetComponent<TankLifeB>();
                tankLife.TakeDamage(1);
                SpawnAoE();
                DestroyOnContact(0f);
            }
        }

        if (collision.CompareTag("DestructibleObject") || collision.CompareTag("Bullet"))
        {
            SpawnAoE();
            Destroy(collision.gameObject);
            DestroyOnContact(0f);
        }

        if (collision.CompareTag("Explosion"))
        {
            if (!isExplosing)
            {
                SpawnAoE();
                isExplosing = true;
            }
            DestroyOnContact(0f);
        }

    }

    private GameObject FindUnusedExplosion()
    {
        foreach (var item in explosionPool.ExplosionArray)
        {
            ExplosionDamage tempoUsed = item.GetComponent<ExplosionDamage>();
            if (!tempoUsed.isUsed)
            {
                tempoUsed.isUsed = true;
                return item;
            }
        }
        return null;
    }

    private void SetUpExplosion(Vector3 _pos)
    {
        GameObject tempo = Instantiate(explosionPrefab, _pos, Quaternion.identity);
        tempo.GetComponent<ExplosionDamage>().bulletParent = this;

        switch (power)
        {
            case TankStateB.TankPowerBullet.levelOne:
                break;
            case TankStateB.TankPowerBullet.levelTwo:
                tempo.transform.localScale = new Vector3(2f, 2f, 1f);
                break;
            case TankStateB.TankPowerBullet.levelThree:
                tempo.transform.localScale = new Vector3(4f, 4f, 1f);
                break;
            case TankStateB.TankPowerBullet.levelFour:
                tempo.transform.localScale = new Vector3(6f, 6f, 1f);
                break;
            default:
                break;
        }
    }

    private void SetUpExplosionPool(Vector3 _pos, GameObject tempo)
    {
        if (!tempo)
        {
            tempo = Instantiate(explosionPrefab, _pos, Quaternion.identity);
            tempo.GetComponent<ExplosionDamage>().mustBeDestroy = true;
        }
        else
            tempo.transform.position = _pos;

        tempo.SetActive(true);
        tempo.GetComponent<ExplosionDamage>().bulletParent = this;

        switch (power)
        {
            case TankStateB.TankPowerBullet.levelOne:
                break;
            case TankStateB.TankPowerBullet.levelTwo:
                tempo.transform.localScale = new Vector3(2f, 2f, 1f);
                break;
            case TankStateB.TankPowerBullet.levelThree:
                tempo.transform.localScale = new Vector3(4f, 4f, 1f);
                break;
            case TankStateB.TankPowerBullet.levelFour:
                tempo.transform.localScale = new Vector3(6f, 6f, 1f);
                break;
            default:
                break;
        }
    }

    public void SpawnAoE()
    {
        SetUpExplosionPool(transform.position, FindUnusedExplosion());

        for (int i = 1; i < sizeAoE; i++)
        {
            SetUpExplosionPool(transform.position + Vector3.left * i, FindUnusedExplosion());
            SetUpExplosionPool(transform.position + Vector3.up * i, FindUnusedExplosion());
            SetUpExplosionPool(transform.position + Vector3.down * i, FindUnusedExplosion());
            SetUpExplosionPool(transform.position + Vector3.right * i, FindUnusedExplosion());
        }
    }

    public void DestroyOnContact(float timer)
    {
        RetrieveAmmo?.Invoke(id);
        StopAllCoroutines();
        Destroy(gameObject, timer);
    }
}
