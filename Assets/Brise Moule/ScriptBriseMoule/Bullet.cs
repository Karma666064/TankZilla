using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuration")]
    public float speed = 10f;
    public float lifetime = 5f;
    
    [HideInInspector]
    public int ownerPlayer;
    
    [HideInInspector]
    public Vector2 direction;
    
    private Rigidbody2D rb;
    private bool isDestroyed = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        Destroy(gameObject, lifetime);
    }
    void OnCollisionEnter2D(Collision2D collision)
{
    Debug.Log($"OnCollision détecté avec {collision.gameObject.name}");
}
    
    public void Initialize(int playerNum, Vector2 dir)
    {
        ownerPlayer = playerNum;
        direction = dir.normalized;
        
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        
        if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        
        gameObject.tag = "Bullet";
        
        Debug.Log($"Balle créée par P{playerNum}, direction: {direction}");
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;
        
        if (collision.CompareTag("Bullet"))
        {
            Bullet otherBullet = collision.GetComponent<Bullet>();
            
            if (otherBullet != null && !otherBullet.isDestroyed)
            {
                if (otherBullet.ownerPlayer != ownerPlayer)
                {
                    Debug.Log($"Collision ! P{ownerPlayer} vs P{otherBullet.ownerPlayer}");
                    Debug.Log($"Direction 1: {direction}, Direction 2: {otherBullet.direction}");
                    
                    float dot = Vector2.Dot(direction, otherBullet.direction);
                    Debug.Log($"Dot product: {dot}");
                    
                    if (dot < 0)
                    {
                        Debug.Log("→ Balles opposées, destruction !");
                        
                        isDestroyed = true;
                        otherBullet.isDestroyed = true;
                        
                        CreateExplosionEffect();
                        
                        Destroy(otherBullet.gameObject);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.Log("→ Même direction, pas de destruction");
                    }
                }
            }
        }
        
        if (collision.CompareTag("Wall"))
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    
    void CreateExplosionEffect()
    {
        GameObject explosion = new GameObject("Explosion");
        explosion.transform.position = transform.position;
        
        SpriteRenderer sr = explosion.AddComponent<SpriteRenderer>();
        
        if (GetComponent<SpriteRenderer>() != null)
        {
            sr.sprite = GetComponent<SpriteRenderer>().sprite;
        }
        
        sr.color = Color.yellow;
        sr.sortingOrder = 10;
        
        explosion.AddComponent<ExplosionEffect>();
    }
}

public class ExplosionEffect : MonoBehaviour
{
    private float duration = 0.3f;
    private float timer = 0f;
    private SpriteRenderer sr;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.one * 0.5f;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        float progress = timer / duration;
        transform.localScale = Vector3.one * (0.5f + progress * 1.5f);
        
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f - progress;
            sr.color = c;
        }
        
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}