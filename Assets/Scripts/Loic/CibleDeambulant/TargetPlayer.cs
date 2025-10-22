using UnityEngine;
using UnityEngine.InputSystem;

public class TargetPlayer : MonoBehaviour
{
    [Header("Configuration")]
    public int playerNumber = 1;
    public float shootCooldown = 0.3f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float minX = -8f; // Limite gauche
    public float maxX = 8f;  // Limite droite
    
    [Header("Input System")]
    public InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    
    [Header("Stats")]
    public int score = 0;
    
    private bool canShoot = true;
    private float shootTimer = 0f;
    private bool isGameActive = false;
    private float moveInput = 0f;
    
    // Direction de tir
    private Vector2 shootDirection;
    
    void Awake()
    {
        if (inputActions != null)
        {
            string actionMapName = playerNumber == 1 ? "KataPlayer1" : "KataPlayer2";
            playerActionMap = inputActions.FindActionMap(actionMapName);
            
            if (playerActionMap != null)
            {
                // Vérifier que les actions existent avant de s'y abonner
                InputAction fireAction = playerActionMap.FindAction("Fire");
                if (fireAction != null)
                {
                    fireAction.performed += OnFire;
                }
                else
                {
                    Debug.LogError($"Action 'Fire' non trouvée dans {actionMapName} !");
                }
                
                InputAction moveAction = playerActionMap.FindAction("Move");
                if (moveAction != null)
                {
                    moveAction.performed += OnMove;
                    moveAction.canceled += OnMove;
                }
                else
                {
                    Debug.LogError($"Action 'Move' non trouvée dans {actionMapName} !");
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
            Debug.LogError("Input Actions Asset non assigné !");
        }
        
        // Player 1 tire vers le haut, Player 2 tire vers le haut aussi
        // (les cibles sont au-dessus des joueurs)
        shootDirection = Vector2.up;
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
        // Gérer le déplacement horizontal
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            Vector3 newPos = transform.position;
            newPos.x += moveInput * moveSpeed * Time.deltaTime;
            
            // Appliquer les limites
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            
            transform.position = newPos;
        }
        
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
    
    private void OnMove(InputAction.CallbackContext context)
    {
        if (isGameActive)
        {
            moveInput = context.ReadValue<float>();
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
        
        // Créer le projectile
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        
        // Configurer le projectile
        TargetProjectile projectileScript = projectile.GetComponent<TargetProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(playerNumber, shootDirection);
        }
        
        // Activer le cooldown
        canShoot = false;
        shootTimer = shootCooldown;
        
        // Feedback visuel
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
        
        // Ne pas descendre en dessous de 0
        if (score < 0)
            score = 0;
        
        Debug.Log($"Player {playerNumber} score: {score} ({(points >= 0 ? "+" : "")}{points})");
        
        // Notifier le GameManager pour mettre à jour l'UI
        if (TargetGameManager.Instance != null)
        {
            TargetGameManager.Instance.UpdateScores();
        }
    }
    
    public void RemoveScore(int points)
    {
        AddScore(-points); // Appeler AddScore avec une valeur négative
        
        // Effet visuel de pénalité
        StartCoroutine(PenaltyEffect());
    }
    
    System.Collections.IEnumerator PenaltyEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color original = sr.color;
            
            // Clignoter en rouge
            for (int i = 0; i < 3; i++)
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                sr.color = original;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    public void StartGame()
    {
        isGameActive = true;
        score = 0;
        canShoot = true;
        shootTimer = 0f;
        moveInput = 0f;
    }
    
    public void StopGame()
    {
        isGameActive = false;
        moveInput = 0f;
    }
    
    void OnDestroy()
    {
        if (playerActionMap != null)
        {
            // Vérifier que chaque action existe avant de se désabonner
            InputAction fireAction = playerActionMap.FindAction("Fire");
            if (fireAction != null)
            {
                fireAction.performed -= OnFire;
            }
            
            InputAction moveAction = playerActionMap.FindAction("Move");
            if (moveAction != null)
            {
                moveAction.performed -= OnMove;
                moveAction.canceled -= OnMove;
            }
            
            playerActionMap.Disable();
        }
    }
}