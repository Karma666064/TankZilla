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

    private int numberGamePlayed = 0;

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
        //CheckAllGamePlayed();
        Debug.Log(numberGamePlayed);
        if (numberGamePlayed < 5)
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
            numberGamePlayed = 0;
            SceneManager.LoadScene("MenuScene");
        }
    }
    
    private void ResetGameList()
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

    private void CheckAllGamePlayed()
    {
        bool tempo = true;

        foreach (KeyValuePair<string, bool> pair in gameList)
        {
            Debug.Log("Value " + pair.Value);
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
                    numberGamePlayed += 1;
                    SceneManager.LoadScene("BriseMoule");
                    gameList["BriseMoule"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
            case 2:
                if (!gameList["CibleDeambulant"])
                {
                    numberGamePlayed += 1;
                    SceneManager.LoadScene("CibleDeambulant");
                    gameList["CibleDeambulant"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
            case 3:
                if (!gameList["CourseBarjot"])
                {
                    numberGamePlayed += 1;
                    SceneManager.LoadScene("CourseBarjotGameScene");
                    gameList["CourseBarjot"] = true;
                }
                else
                    ChooseLoicGames(Random.Range(1, 4));
                break;
            default:
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
                    numberGamePlayed += 1;
                    SceneManager.LoadScene("FootTankGameScene");
                    gameList["TankFoot"] = true;
                }
                else
                    ChooseSungoGames(Random.Range(1, 3));
                break;
            case 2:
                if (!gameList["BomberTank"])
                {
                    numberGamePlayed += 1;
                    SceneManager.LoadScene("BomberTankGameScene");
                    gameList["BomberTank"] = true;
                }
                else
                    ChooseSungoGames(Random.Range(1, 3));
                break;
            default:
                break;
        }
    }
}
