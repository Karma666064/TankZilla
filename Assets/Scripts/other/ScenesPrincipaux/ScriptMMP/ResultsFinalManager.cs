using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ResultsFinalManager : MonoBehaviour
{
    [Header("UI Général")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI winnerAnnouncementText;
    public GameObject player1WinnerPanel;
    public GameObject player2WinnerPanel;

    [Header("Scores Globaux")]
    public TextMeshProUGUI player1TotalWinsText;
    public TextMeshProUGUI player2TotalWinsText;

    [Header("Détails des Mini-Jeux")]
    public Transform scoreListContainer;
    public GameObject scoreEntryPrefab; // Prefab pour afficher chaque score

    [Header("Boutons")]
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button quitButton;

    [Header("Effets Visuels")]
    public ParticleSystem confettiEffect;
    public AudioSource victorySound;

    void Start()
    {
        DisplayResults();
        
        // Configuration des boutons
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(OnPlayAgain);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenu);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuit);
    }

    void DisplayResults()
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("GameDataManager non trouvé !");
            return;
        }

        GameDataManager gdm = GameDataManager.Instance;

        // Afficher le gagnant global
        int winner = gdm.GetOverallWinner();
        
        if (winnerAnnouncementText != null)
        {
            if (winner == 1)
            {
                winnerAnnouncementText.text = "🏆 JOUEUR 1 REMPORTE LA VICTOIRE ! 🏆";
                winnerAnnouncementText.color = Color.blue;
            }
            else if (winner == 2)
            {
                winnerAnnouncementText.text = "🏆 JOUEUR 2 REMPORTE LA VICTOIRE ! 🏆";
                winnerAnnouncementText.color = Color.red;
            }
            else
            {
                winnerAnnouncementText.text = "🤝 ÉGALITÉ PARFAITE ! 🤝";
                winnerAnnouncementText.color = Color.yellow;
            }
        }

        // Afficher les panneaux de victoire
        if (player1WinnerPanel != null)
            player1WinnerPanel.SetActive(winner == 1);
        
        if (player2WinnerPanel != null)
            player2WinnerPanel.SetActive(winner == 2);

        // Afficher les victoires totales
        if (player1TotalWinsText != null)
            player1TotalWinsText.text = $"Victoires: {gdm.player1TotalWins}";
        
        if (player2TotalWinsText != null)
            player2TotalWinsText.text = $"Victoires: {gdm.player2TotalWins}";

        // Afficher les détails de chaque mini-jeu
        DisplayMiniGameScores();

        // Effets visuels et sonores
        if (winner != 0)
        {
            if (confettiEffect != null)
                confettiEffect.Play();
            
            if (victorySound != null)
                victorySound.Play();
        }
    }

    void DisplayMiniGameScores()
    {
        if (scoreListContainer == null || scoreEntryPrefab == null)
            return;

        // Nettoyer les anciennes entrées
        foreach (Transform child in scoreListContainer)
        {
            Destroy(child.gameObject);
        }

        // Créer une entrée pour chaque mini-jeu
        foreach (var score in GameDataManager.Instance.scores)
        {
            GameObject entry = Instantiate(scoreEntryPrefab, scoreListContainer);
            
            // Configurer l'entrée (adapter selon votre prefab)
            TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();
            
            if (texts.Length >= 4)
            {
                texts[0].text = FormatGameName(score.gameName);
                texts[1].text = $"P1: {score.player1Score}";
                texts[2].text = $"P2: {score.player2Score}";
                
                if (score.winnerPlayerNumber == 1)
                {
                    texts[3].text = "Gagnant: Joueur 1";
                    texts[3].color = Color.blue;
                }
                else if (score.winnerPlayerNumber == 2)
                {
                    texts[3].text = "Gagnant: Joueur 2";
                    texts[3].color = Color.red;
                }
                else
                {
                    texts[3].text = "Égalité";
                    texts[3].color = Color.yellow;
                }
            }
        }
    }

    string FormatGameName(string gameName)
    {
        // Convertir "BriseMoule" en "Brise Moule"
        return System.Text.RegularExpressions.Regex.Replace(gameName, "([a-z])([A-Z])", "$1 $2");
    }

    void OnPlayAgain()
    {
        GameDataManager.Instance.ResetGame();
        string firstGame = GameDataManager.Instance.GetNextMiniGameScene();
        SceneManager.LoadScene(firstGame);
    }

    void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}