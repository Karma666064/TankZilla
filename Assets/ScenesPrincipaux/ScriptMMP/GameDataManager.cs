using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [System.Serializable]
    public class MiniGameScore
    {
        public string gameName;
        public int player1Score;
        public int player2Score;
        public int winnerPlayerNumber; // 0 = égalité, 1 ou 2 = gagnant
        public float completionTime;
    }

    [Header("Configuration")]
    public List<string> miniGameScenes = new List<string>
    {
        "BriseMoule",
        "CibleDeambulant",
        "CourseBarjot"
    };

    [Header("Données de jeu")]
    public List<MiniGameScore> scores = new List<MiniGameScore>();
    public int currentMiniGameIndex = 0;
    public int player1TotalWins = 0;
    public int player2TotalWins = 0;

    void Awake()
    {
        // Singleton persistant
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeScores()
    {
        scores.Clear();
        foreach (string gameName in miniGameScenes)
        {
            scores.Add(new MiniGameScore
            {
                gameName = gameName,
                player1Score = 0,
                player2Score = 0,
                winnerPlayerNumber = 0,
                completionTime = 0f
            });
        }
    }

    public void SaveMiniGameResult(string gameName, int player1Score, int player2Score, int winner, float time = 0f)
    {
        MiniGameScore gameScore = scores.Find(s => s.gameName == gameName);
        
        if (gameScore != null)
        {
            gameScore.player1Score = player1Score;
            gameScore.player2Score = player2Score;
            gameScore.winnerPlayerNumber = winner;
            gameScore.completionTime = time;

            // Incrémenter le compteur de victoires
            if (winner == 1)
                player1TotalWins++;
            else if (winner == 2)
                player2TotalWins++;

            Debug.Log($"Score sauvegardé pour {gameName}: P1={player1Score}, P2={player2Score}, Gagnant={winner}");
        }
        else
        {
            Debug.LogError($"Mini-jeu '{gameName}' non trouvé dans la liste !");
        }
    }

    public MiniGameScore GetScore(string gameName)
    {
        return scores.Find(s => s.gameName == gameName);
    }

    public string GetNextMiniGameScene()
    {
        if (currentMiniGameIndex < miniGameScenes.Count)
        {
            string nextScene = miniGameScenes[currentMiniGameIndex];
            currentMiniGameIndex++;
            return nextScene;
        }
        
        // Si tous les mini-jeux sont terminés, aller aux résultats
        return "ResultatFinal";
    }

    public bool AllGamesCompleted()
    {
        return currentMiniGameIndex >= miniGameScenes.Count;
    }

    public int GetOverallWinner()
    {
        if (player1TotalWins > player2TotalWins)
            return 1;
        else if (player2TotalWins > player1TotalWins)
            return 2;
        else
            return 0; // Égalité
    }

    public void ResetGame()
    {
        currentMiniGameIndex = 0;
        player1TotalWins = 0;
        player2TotalWins = 0;
        InitializeScores();
    }
}