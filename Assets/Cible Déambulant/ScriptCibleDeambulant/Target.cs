using UnityEngine;
using TMPro;
using System.Collections;

public class Target : MonoBehaviour
{
    [Header("Configuration")]
    public int targetOwner = 1; // Quel joueur peut toucher cette cible
    public int minPoints = 10;
    public int maxPoints = 100;
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    
    [Header("Zone de Mouvement")]
    public float moveRangeX = 3f;
    public float moveRangeY = 2f;
    
    [Header("Références")]
    public TextMeshPro pointsText;
    public SpriteRenderer targetRenderer;
    
    private int currentPoints;
    private Vector3 startPosition;
    private Vector2 moveDirection;
    private float changeDirectionTimer;
    
    void Start()
    {
        startPosition = transform.position;
        
        // Choisir des points aléatoires
        RandomizePoints();
        
        // Choisir une direction de mouvement aléatoire
        ChooseNewDirection();
        changeDirectionTimer = changeDirectionInterval;
    }
    
    void Update()
    {
        // Mouvement aléatoire de la cible
        MoveTarget();
        
        // Changer de direction périodiquement
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            ChooseNewDirection();
            changeDirectionTimer = changeDirectionInterval;
        }
    }
    
    void MoveTarget()
    {
        // Déplacer la cible
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
        
        // Limites de mouvement (rebondir sur les bords)
        Vector3 offset = transform.position - startPosition;
        
        if (Mathf.Abs(offset.x) > moveRangeX)
        {
            moveDirection.x *= -1;
            transform.position = startPosition + new Vector3(
                Mathf.Sign(offset.x) * moveRangeX,
                offset.y,
                0
            );
        }
        
        if (Mathf.Abs(offset.y) > moveRangeY)
        {
            moveDirection.y *= -1;
            transform.position = startPosition + new Vector3(
                offset.x,
                Mathf.Sign(offset.y) * moveRangeY,
                0
            );
        }
    }
    
    void ChooseNewDirection()
    {
        moveDirection = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }
    
    void RandomizePoints()
    {
        // Générer des points aléatoires
        currentPoints = Random.Range(minPoints / 10, maxPoints / 10) * 10; // Multiples de 10
        
        // Mettre à jour l'affichage
        if (pointsText != null)
        {
            pointsText.text = currentPoints.ToString();
        }
        
        // Changer la couleur selon la valeur
        if (targetRenderer != null)
        {
            float t = (float)(currentPoints - minPoints) / (maxPoints - minPoints);
            targetRenderer.color = Color.Lerp(Color.white, Color.yellow, t);
        }
    }
    
    public void OnHit(int playerNum)
    {
        // Vérifier que c'est le bon joueur
        if (playerNum != targetOwner) return;
        
        Debug.Log($"Cible touchée par P{playerNum} ! +{currentPoints} points");
        
        // Donner les points au joueur
        TargetPlayer player = FindPlayerByNumber(playerNum);
        if (player != null)
        {
            player.AddScore(currentPoints);
        }
        
        // Effet visuel
        StartCoroutine(HitEffect(true));
        
        // Changer les points de la cible
        RandomizePoints();
    }
    
    public void OnWrongHit(int playerNum)
    {
        // Le joueur a touché la mauvaise cible
        Debug.Log($"Mauvaise cible touchée par P{playerNum} ! -{currentPoints} points");
        
        // Retirer les points au joueur
        TargetPlayer player = FindPlayerByNumber(playerNum);
        if (player != null)
        {
            player.RemoveScore(currentPoints);
        }
        
        // Effet visuel de pénalité
        StartCoroutine(HitEffect(false));
    }
    
    TargetPlayer FindPlayerByNumber(int playerNum)
    {
        if (TargetGameManager.Instance != null)
        {
            if (playerNum == 1)
                return TargetGameManager.Instance.player1;
            else
                return TargetGameManager.Instance.player2;
        }
        return null;
    }
    
    IEnumerator HitEffect(bool isCorrectHit)
    {
        // Effet de flash
        if (targetRenderer != null)
        {
            Color original = targetRenderer.color;
            Color flashColor = isCorrectHit ? Color.green : Color.red;
            
            for (int i = 0; i < 2; i++)
            {
                targetRenderer.color = flashColor;
                yield return new WaitForSeconds(0.05f);
                targetRenderer.color = Color.white;
                yield return new WaitForSeconds(0.05f);
            }
            
            // Restaurer la couleur basée sur les points (si bon hit)
            if (isCorrectHit)
            {
                float t = (float)(currentPoints - minPoints) / (maxPoints - minPoints);
                targetRenderer.color = Color.Lerp(Color.white, Color.yellow, t);
            }
            else
            {
                targetRenderer.color = original;
            }
        }
        
        // Petit effet de scale
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.2f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = originalScale;
    }
}