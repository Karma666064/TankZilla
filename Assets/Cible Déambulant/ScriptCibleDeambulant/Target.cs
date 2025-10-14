using UnityEngine;
using TMPro;
using System.Collections;

public class Target : MonoBehaviour
{
    [Header("Configuration")]
    public int targetOwner = 1;
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
    
        RandomizePoints();
        
        ChooseNewDirection();
        changeDirectionTimer = changeDirectionInterval;
    }
    
    void Update()
    {
        MoveTarget();
        
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0f)
        {
            ChooseNewDirection();
            changeDirectionTimer = changeDirectionInterval;
        }
    }
    
    void MoveTarget()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
        
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
        currentPoints = Random.Range(minPoints / 10, maxPoints / 10) * 10; // Multiples de 10
        
        if (pointsText != null)
        {
            pointsText.text = currentPoints.ToString();
        }
        
        if (targetRenderer != null)
        {
            float t = (float)(currentPoints - minPoints) / (maxPoints - minPoints);
            targetRenderer.color = Color.Lerp(Color.white, Color.yellow, t);
        }
    }
    
    public void OnHit(int playerNum)
    {
        if (playerNum != targetOwner) return;
        
        Debug.Log($"Cible touchée par P{playerNum} ! +{currentPoints} points");
        
        TargetPlayer player = FindPlayerByNumber(playerNum);
        if (player != null)
        {
            player.AddScore(currentPoints);
        }
        
        StartCoroutine(HitEffect());
        
        RandomizePoints();
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
    
    IEnumerator HitEffect()
    {
        if (targetRenderer != null)
        {
            Color original = targetRenderer.color;
            
            for (int i = 0; i < 2; i++)
            {
                targetRenderer.color = Color.green;
                yield return new WaitForSeconds(0.05f);
                targetRenderer.color = Color.white;
                yield return new WaitForSeconds(0.05f);
            }
            
            float t = (float)(currentPoints - minPoints) / (maxPoints - minPoints);
            targetRenderer.color = Color.Lerp(Color.white, Color.yellow, t);
        }
        
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.2f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = originalScale;
    }
}