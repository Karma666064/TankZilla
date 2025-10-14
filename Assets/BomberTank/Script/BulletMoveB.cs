using System;
using System.Data.Common;
using UnityEngine;

public class BulletMoveB : MonoBehaviour
{
    public static event Action<int, int> AddScore;
    [SerializeField] private float speed = 100;
    public Vector3 directionBullet;
    public int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //id = 0;
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * directionBullet;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("In Wall");
            Destroy(gameObject);
        }

        if (collision.CompareTag("Tank"))
        {
            if (collision.GetComponent<TankStateB>().id != id)
            {
                TankLifeB tankLife = collision.GetComponent<TankLifeB>();
                if (!tankLife.isBlinking)
                    AddScore?.Invoke(id, 100);
                tankLife.TakeDamage(1);
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("DestructibleObject"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
