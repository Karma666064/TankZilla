using UnityEngine;

public class TargetProjectile : MonoBehaviour
{
    [Header("Configuration")]
    public float speed = 15f;
    public float lifetime = 3f;
    
    [HideInInspector]
    public int ownerPlayer;
    
    private Vector2 direction;
    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    public void Initialize(int playerNum, Vector2 dir)
    {
        ownerPlayer = playerNum;
        direction = dir.normalized;
        
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        
        gameObject.tag = "Projectile";
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            Target target = collision.GetComponent<Target>();
            
            if (target != null)
            {
                if (target.targetOwner == ownerPlayer)
                {
                    target.OnHit(ownerPlayer);
                    
                    Destroy(gameObject);
                }
            }
        }
        
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}