using UnityEngine;
using TMPro;
using System.Collections;

public class DuelGameManager : MonoBehaviour
{
    public static DuelGameManager Instance;
    
    [Header("Joueurs")]
    public PlayerDuel player1;
    public PlayerDuel player2;
    
    [Header("UI")]
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    
    [Header("Configuration")]
    public float gameDuration = 30f; // Durée de la partie en secondes
    
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
        
        // Démarrer le compte à rebours
        StartCoroutine(StartCountdown());
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
        // Mettre à jour les scores
        UpdateScoreDisplay();
        
        // Timer de jeu (optionnel - fin par timer)
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            EndGameByTimeout();
        }
    }
    
    IEnumerator StartCountdown()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            
            // Compte à rebours
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                countdownText.fontSize = 120;
                countdownText.color = Color.yellow;
                yield return new WaitForSeconds(1f);
            }
            
            // GO !
            countdownText.text = "FIRE !";
            countdownText.fontSize = 150;
            countdownText.color = Color.red;
            yield return new WaitForSeconds(0.8f);
            
            countdownText.gameObject.SetActive(false);
        }
        
        // Démarrer le jeu
        StartGame();
    }
    
    void StartGame()
    {
        isGameActive = true;
        gameTimer = gameDuration;
        
        if (player1 != null) player1.StartGame();
        if (player2 != null) player2.StartGame();
        
        Debug.Log("Duel commencé !");
    }
    
    void UpdateScoreDisplay()
    {
        if (player1ScoreText != null && player1 != null)
        {
            player1ScoreText.text = $"P1: {player1.bulletsShot}";
        }
        
        if (player2ScoreText != null && player2 != null)
        {
            player2ScoreText.text = $"P2: {player2.bulletsShot}";
        }
    }
    
    public void PlayerDied(int playerNumber)
    {
        if (!isGameActive) return;
        
        isGameActive = false;
        
        // Le joueur adverse gagne
        int winner = playerNumber == 1 ? 2 : 1;
        
        ShowResult($"Joueur {winner} a gagné !\nJoueur {playerNumber} a été touché !");
    }
    
    void EndGameByTimeout()
    {
        isGameActive = false;
        
        // Comparer les scores
        int p1Score = player1 != null ? player1.bulletsShot : 0;
        int p2Score = player2 != null ? player2.bulletsShot : 0;
        
        string message;
        if (p1Score > p2Score)
        {
            message = $"Joueur 1 gagne !\n{p1Score} vs {p2Score} balles";
        }
        else if (p2Score > p1Score)
        {
            message = $"Joueur 2 gagne !\n{p2Score} vs {p1Score} balles";
        }
        else
        {
            message = $"Égalité !\n{p1Score} balles chacun";
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