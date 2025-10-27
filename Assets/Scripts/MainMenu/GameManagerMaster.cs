using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerMaster : MonoBehaviour
{
    public static GameManagerMaster Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    enum GameType
    {
        LoicGame,
        SungoGame,
    }

    private Dictionary<string, bool> gameList;

    private GameType currentGame = GameType.LoicGame;

    private bool allGamePlayed = false;

    public int pointPlayerOne = 0;
    public int pointPlayerTwo = 0;

    public string playerOneName;
    public string playerTwoName;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameList = new Dictionary<string, bool>
        {
            { "BriseMoule", false },
            { "CibleDeambulant", false },
            { "CourseBarjot", false },
            { "TankFoot", false },
            { "BomberTank", false }
        };
    }

    public void AddPoint(int id)
    {
        if (id == 1)
        {
            pointPlayerOne += 1;
        } else if (id == 2)
        {
            pointPlayerTwo += 1;
        }
    }

    public void ChooseGame()
    {
        CheckAllGamePlayed();
        if (!allGamePlayed)
        {
            if (currentGame == GameType.LoicGame)
            {
                currentGame = GameType.SungoGame;
                ChooseLoicGames(Random.Range(1, 4));
            }
            else
            {
                currentGame = GameType.LoicGame;
                ChooseSungoGames(Random.Range(1, 3));
            }
        }
        else
        {
            ResetGameList();
        }
    }
    
    private void ResetGameList()
    {
        foreach (KeyValuePair<string, bool> pair in gameList)
        {
            gameList[pair.Key] = false;
        }
    }

    private void CheckAllGamePlayed()
    {
        bool tempo = true;

        foreach (KeyValuePair<string, bool> pair in gameList)
        {
            if (!pair.Value)
            {
                tempo = false;
                break;
            }
        }

        allGamePlayed = tempo;
    }

    private void ChooseLoicGames(int rand)
    {
        switch (rand)
        {
            case 1:
                if (!gameList["BriseMoule"])
                {
                    SceneManager.LoadScene("BriseMoule");
                    gameList["BriseMoule"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
            case 2:
                if (!gameList["CibleDeambulant"])
                {
                    SceneManager.LoadScene("CibleDeambulant");
                    gameList["CibleDeambulant"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
            case 3:
                if (!gameList["CourseBarjot"])
                {
                    SceneManager.LoadScene("CourseBarjotGameScene");
                    gameList["CourseBarjot"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
        }
    }
    
    private void ChooseSungoGames(int rand)
    {
         switch (rand)
        {
            case 1:
                if (!gameList["TankFoot"])
                {
                    SceneManager.LoadScene("FootTankGameScene");
                    gameList["TankFoot"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
            case 2:
                if (!gameList["BomberTank"])
                {
                    SceneManager.LoadScene("BomberTankGameScene");
                    gameList["BomberTank"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
        }
    }
}
