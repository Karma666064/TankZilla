using UnityEngine;

public class BulletMoveB : MonoBehaviour
{
    [SerializeField] private float speed = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.up * Time.fixedDeltaTime * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
         Debug.Log(collision.tag);
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("In Wall");
            Destroy(gameObject);
        }
    }
}
