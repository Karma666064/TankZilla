using System.Collections;
using UnityEngine;

public class BulletMoveF : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [HideInInspector] public float lifeTime;
    [HideInInspector] public Vector2 direction;

    void Start()
    {
        StartCoroutine(DespawnBullet(lifeTime));
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
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
