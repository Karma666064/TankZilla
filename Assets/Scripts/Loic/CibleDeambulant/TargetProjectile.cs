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
        // Auto-destruction après un certain temps
        Destroy(gameObject, lifetime);
    }
    
    public void Initialize(int playerNum, Vector2 dir)
    {
        ownerPlayer = playerNum;
        direction = dir.normalized;
        
        // Appliquer la vélocité
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        
        // Tag pour identifier les projectiles
        gameObject.tag = "Projectile";
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifier si on touche une cible
        if (collision.CompareTag("Target"))
        {
            Target target = collision.GetComponent<Target>();
            
            if (target != null)
            {
                // Vérifier que c'est la bonne cible pour ce joueur
                if (target.targetOwner == ownerPlayer)
                {
                    // Faire "hit" sur la cible (gagner des points)
                    target.OnHit(ownerPlayer);
                }
                else
                {
                    // Mauvaise cible ! Pénalité
                    Debug.Log($"P{ownerPlayer} a touché la mauvaise cible !");
                    target.OnWrongHit(ownerPlayer);
                }
                
                // Détruire le projectile dans tous les cas
                Destroy(gameObject);
            }
        }
        
        // Détruire si on touche un mur
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}