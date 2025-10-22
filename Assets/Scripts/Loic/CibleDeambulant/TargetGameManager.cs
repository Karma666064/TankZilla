using UnityEngine;
using TMPro;
using System.Collections;

public class TargetGameManager : MonoBehaviour
{
    public static TargetGameManager Instance;
    
    [Header("Joueurs")]
    public TargetPlayer player1;
    public TargetPlayer player2;
    
    [Header("UI")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    
    [Header("Configuration")]
    public float gameDuration = 60f;
    
    private bool isGameActive = false;
    private float gameTimer;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
        
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
        
        StartCoroutine(StartCountdown());
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
        gameTimer -= Time.deltaTime;
        
        UpdateTimerDisplay();
        
        if (gameTimer <= 0)
        {
            EndGame();
        }
    }
    
    IEnumerator StartCountdown()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                countdownText.fontSize = 120;
                countdownText.color = Color.yellow;
                yield return new WaitForSeconds(1f);
            }
            
            countdownText.text = "SHOOT !";
            countdownText.fontSize = 150;
            countdownText.color = Color.green;
            yield return new WaitForSeconds(0.8f);
            
            countdownText.gameObject.SetActive(false);
        }
        
        StartGame();
    }
    
    void StartGame()
    {
        isGameActive = true;
        gameTimer = gameDuration;
        
        if (player1 != null) player1.StartGame();
        if (player2 != null) player2.StartGame();
        
        UpdateScores();
        
        Debug.Log("Jeu de tir sur cibles commencé !");
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(gameTimer);
            timerText.text = $"Temps: {seconds}s";
            
            if (seconds <= 10)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }
    
    public void UpdateScores()
    {
        if (player1ScoreText != null && player1 != null)
        {
            player1ScoreText.text = $"P1: {player1.score}";
        }
        
        if (player2ScoreText != null && player2 != null)
        {
            player2ScoreText.text = $"P2: {player2.score}";
        }
    }
    
    void EndGame()
    {
        isGameActive = false;
        
        if (player1 != null) player1.StopGame();
        if (player2 != null) player2.StopGame();
        
        int p1Score = player1 != null ? player1.score : 0;
        int p2Score = player2 != null ? player2.score : 0;
        
        string message;
        if (p1Score > p2Score)
        {
            message = $"JOUEUR 1 GAGNE !\n\n{p1Score} points\nvs\n{p2Score} points";
        }
        else if (p2Score > p1Score)
        {
            message = $"JOUEUR 2 GAGNE !\n\n{p2Score} points\nvs\n{p1Score} points";
        }
        else
        {
            message = $"ÉGALITÉ !\n\n{p1Score} points chacun";
        }
        
        ShowResult(message);
    }
    
    void ShowResult(string message)
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }
        
        if (resultText != null)
        {
            resultText.text = message;
            resultText.fontSize = 50;
            resultText.color = Color.white;
        }
        
        Debug.Log(message);
    }
    
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}