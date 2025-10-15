using System.Collections;
using UnityEngine;

public class BulletMoveF : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed = 12f;
    [HideInInspector] public float lifeTime;
    [HideInInspector] public Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(DespawnBullet(lifeTime));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ball"))
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 reflectDir = Vector2.Reflect(direction, normal);

            direction = reflectDir;
        }
    }

    IEnumerator DespawnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
