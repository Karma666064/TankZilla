using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDuel : MonoBehaviour
{
    [Header("Configuration")]
    public int playerNumber = 1; // 1 ou 2
    public float shootCooldown = 0.2f; // Temps minimum entre deux tirs
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    
    [Header("Input System")]
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    
    [Header("Stats")]
    public int bulletsShot = 0;
    
    private bool canShoot = true;
    private float shootTimer = 0f;
    private bool isGameActive = false;
    private bool isDead = false;
    
    // Direction du tir selon le joueur
    private Vector2 shootDirection;
    
    void Awake()
    {
        if (inputActions != null)
        {
            string actionMapName = playerNumber == 1 ? "KataPlayer1" : "KataPlayer2";
            playerActionMap = inputActions.FindActionMap(actionMapName);
            
            if (playerActionMap != null)
            {
                // Vérifier que l'action Fire existe
                InputAction fireAction = playerActionMap.FindAction("Fire");
                if (fireAction != null)
                {
                    fireAction.performed += OnFire;
                }
                else
                {
                    Debug.LogError($"Action 'Fire' non trouvée dans {actionMapName} !");
                }
                
                playerActionMap.Enable();
            }
            else
            {
                Debug.LogError($"Action Map '{actionMapName}' non trouvé !");
            }
        }
        else
        {
            Debug.LogError("Input Actions Asset non assigné sur PlayerDuel !");
        }
        
        // Player 1 tire vers la droite, Player 2 tire vers la gauche
        shootDirection = playerNumber == 1 ? Vector2.right : Vector2.left;
    }
    
    void Update()
    {
        if (!isGameActive || isDead) return;
        
        // Gérer le cooldown de tir
        if (!canShoot)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                canShoot = true;
            }
        }
    }
    
    private void OnFire(InputAction.CallbackContext context)
    {
        if (isGameActive && !isDead && canShoot)
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;
        
        // Créer la balle
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        
        // Configurer la balle
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(playerNumber, shootDirection);
        }
        
        // Incrémenter le compteur
        bulletsShot++;
        
        // Activer le cooldown
        canShoot = false;
        shootTimer = shootCooldown;
        
        // Feedback visuel (optionnel)
        StartCoroutine(ShootEffect());
        
        Debug.Log($"Player {playerNumber} tire ! Total: {bulletsShot} balles");
    }
    
    System.Collections.IEnumerator ShootEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color original = sr.color;
            sr.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
            sr.color = original;
        }
    }
    
    public void StartGame()
    {
        isGameActive = true;
        isDead = false;
        bulletsShot = 0;
        canShoot = true;
        shootTimer = 0f;
    }
    
    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        isGameActive = false;
        
        // Effet visuel de mort
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
        }
        
        Debug.Log($"Player {playerNumber} est mort !");
        
        // Notifier le GameManager
        if (DuelGameManager.Instance != null)
        {
            DuelGameManager.Instance.PlayerDied(playerNumber);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si une balle ennemie touche le joueur
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.ownerPlayer != playerNumber)
            {
                Die();
                Destroy(collision.gameObject); // Détruire la balle
            }
        }
    }
    
    void OnDestroy()
    {
        if (playerActionMap != null)
        {
            // Vérifier que l'action existe avant de se désabonner
            InputAction fireAction = playerActionMap.FindAction("Fire");
            if (fireAction != null)
            {
                fireAction.performed -= OnFire;
            }
            
            playerActionMap.Disable();
        }
    }
}