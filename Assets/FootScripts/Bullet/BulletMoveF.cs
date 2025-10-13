using System.Collections;
using UnityEngine;

public class BulletMoveF : MonoBehaviour
{
    [SerializeField] float speed = 3f;
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

    IEnumerator DespawnBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
