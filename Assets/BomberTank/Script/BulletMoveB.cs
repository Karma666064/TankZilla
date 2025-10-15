using System;
using System.Data.Common;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class BulletMoveB : MonoBehaviour
{
    public static event Action<int, int> AddScore;
    public static event Action<int> RetrieveAmmo;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speed = 100;
    public Vector3 directionBullet;
    public int id;
    public TankStateB.TankPowerBullet power;
    public int isAoE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DestroyOnContact(2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * directionBullet;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            if (collision.GetComponent<TankStateB>().id != id)
            {
                TankLifeB tankLife = collision.GetComponent<TankLifeB>();
                if (!tankLife.isBlinking)
                    SendSignalScore();
                tankLife.TakeDamage(1);
                DestroyOnContact(0f);
            }
        }

        if (collision.CompareTag("DestructibleObject"))
        {
            Destroy(collision.gameObject);
            DestroyOnContact(0f);
        }

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

    public void SpawnAoE()
    {
        SetUpExplosion(transform.position);

        int tempoAoECount = isAoE;

        for (int i = 1; i < isAoE; i++)
        {
            SetUpExplosion(transform.position + Vector3.left * i);
            SetUpExplosion(transform.position + Vector3.up * i);
            SetUpExplosion(transform.position + Vector3.down * i);
            SetUpExplosion(transform.position + Vector3.right * i);
        }

        /*if (isAoE >= 2)
        {
            SetUpExplosion(transform.position + Vector3.left);
            SetUpExplosion(transform.position + Vector3.up);
            SetUpExplosion(transform.position + Vector3.down);
            SetUpExplosion(transform.position + Vector3.right);
        }

        if (isAoE >= 3)
        {
            SetUpExplosion(transform.position + Vector3.left * 2);
            SetUpExplosion(transform.position + Vector3.up * 2);
            SetUpExplosion(transform.position + Vector3.down * 2);
            SetUpExplosion(transform.position + Vector3.right * 2);
        }

        if (isAoE >= 4)
        {
            SetUpExplosion(transform.position + Vector3.left * 3);
            SetUpExplosion(transform.position + Vector3.up * 3);
            SetUpExplosion(transform.position + Vector3.down * 3);
            SetUpExplosion(transform.position + Vector3.right * 3);
        }*/
        

    }

    public void DestroyOnContact(float timer)
    {
        SpawnAoE();
        RetrieveAmmo?.Invoke(id);
        Destroy(gameObject, timer);
    }

    public void SendSignalScore()
    {
        AddScore?.Invoke(id, 100);
    }
}
