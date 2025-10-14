using UnityEngine;
using UnityEngine.InputSystem;

public class TargetPlayer : MonoBehaviour
{
    [Header("Configuration")]
    public int playerNumber = 1;
    public float shootCooldown = 0.3f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    
    [Header("Input System")]
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    
    [Header("Stats")]
    public int score = 0;
    
    private bool canShoot = true;
    private float shootTimer = 0f;
    private bool isGameActive = false;
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
        
        shootDirection = Vector2.up;
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
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
        if (isGameActive && canShoot)
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        TargetProjectile projectileScript = projectile.GetComponent<TargetProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(playerNumber, shootDirection);
        }
        
        canShoot = false;
        shootTimer = shootCooldown;
        
        StartCoroutine(ShootEffect());
    }
    
    System.Collections.IEnumerator ShootEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color original = sr.color;
            sr.color = Color.cyan;
            yield return new WaitForSeconds(0.1f);
            sr.color = original;
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Player {playerNumber} score: {score} (+{points})");
        
        if (TargetGameManager.Instance != null)
        {
            TargetGameManager.Instance.UpdateScores();
        }
    }
    
    public void StartGame()
    {
        isGameActive = true;
        score = 0;
        canShoot = true;
        shootTimer = 0f;
    }
    
    public void StopGame()
    {
        isGameActive = false;
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