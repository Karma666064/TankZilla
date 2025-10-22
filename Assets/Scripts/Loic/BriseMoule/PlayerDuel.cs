using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDuel : MonoBehaviour
{
    [Header("Configuration")]
    public int playerNumber = 1;
    public float shootCooldown = 0.2f; 
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
    
    private Vector2 shootDirection;
    
    void Awake()
    {
        if (inputActions != null)
        {
            string actionMapName = playerNumber == 1 ? "Player1" : "Player2";
            playerActionMap = inputActions.FindActionMap(actionMapName);
            
            if (playerActionMap != null)
            {
                playerActionMap.FindAction("Fire").performed += OnFire;
                playerActionMap.Enable();
            }
            else
            {
                Debug.LogError($"Action Map '{actionMapName}' non trouvé !");
            }
        }
        
        shootDirection = playerNumber == 1 ? Vector2.right : Vector2.left;
    }
    
    void Update()
    {
        if (!isGameActive || isDead) return;
        
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
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(playerNumber, shootDirection);
        }
        
        bulletsShot++;
        
        canShoot = false;
        shootTimer = shootCooldown;
        
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
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
        }
        
        Debug.Log($"Player {playerNumber} est mort !");
        
        DuelGameManager.Instance.PlayerDied(playerNumber);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null && bullet.ownerPlayer != playerNumber)
            {
                Die();
                Destroy(collision.gameObject); 
            }
        }
    }
    
    void OnDestroy()
    {
        if (playerActionMap != null)
        {
            playerActionMap.FindAction("Fire").performed -= OnFire;
            playerActionMap.Disable();
        }
    }
}