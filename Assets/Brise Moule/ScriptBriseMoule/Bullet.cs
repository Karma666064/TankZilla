using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Configuration")]
    public float speed = 10f;
    public float lifetime = 5f; // Durée de vie max
    
    [HideInInspector]
    public int ownerPlayer; // Joueur qui a tiré cette balle
    
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

        // Auto-destruction après un certain temps
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
        
        // Appliquer la vélocité
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
        
        // Rotation visuelle optionnelle
        if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        
        // Tag pour les collisions
        gameObject.tag = "Bullet";
        
        Debug.Log($"Balle créée par P{playerNum}, direction: {direction}");
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;
        
        // Collision avec une autre balle
        if (collision.CompareTag("Bullet"))
        {
            Bullet otherBullet = collision.GetComponent<Bullet>();
            
            if (otherBullet != null && !otherBullet.isDestroyed)
            {
                // Vérifier si les balles appartiennent à des joueurs différents
                if (otherBullet.ownerPlayer != ownerPlayer)
                {
                    Debug.Log($"Collision ! P{ownerPlayer} vs P{otherBullet.ownerPlayer}");
                    Debug.Log($"Direction 1: {direction}, Direction 2: {otherBullet.direction}");
                    
                    // Vérifier si les balles vont dans des directions opposées
                    float dot = Vector2.Dot(direction, otherBullet.direction);
                    Debug.Log($"Dot product: {dot}");
                    
                    // Si dot < 0, les balles vont dans des directions opposées
                    if (dot < 0)
                    {
                        Debug.Log("→ Balles opposées, destruction !");
                        
                        // Marquer comme détruit pour éviter double destruction
                        isDestroyed = true;
                        otherBullet.isDestroyed = true;
                        
                        // Effet visuel
                        CreateExplosionEffect();
                        
                        // Détruire les deux balles
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
        
        // Collision avec les murs/limites
        if (collision.CompareTag("Wall"))
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    
    void CreateExplosionEffect()
    {
        // Créer un petit effet visuel d'explosion
        GameObject explosion = new GameObject("Explosion");
        explosion.transform.position = transform.position;
        
        SpriteRenderer sr = explosion.AddComponent<SpriteRenderer>();
        
        // Utiliser un sprite circulaire simple si disponible
        if (GetComponent<SpriteRenderer>() != null)
        {
            sr.sprite = GetComponent<SpriteRenderer>().sprite;
        }
        
        sr.color = Color.yellow;
        sr.sortingOrder = 10;
        
        // Animation simple
        explosion.AddComponent<ExplosionEffect>();
    }
}

// Petit script pour l'effet d'explosion
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
        
        // Agrandir et faire disparaître
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