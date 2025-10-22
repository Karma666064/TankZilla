using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GoalF : MonoBehaviour
{
    EndGameF eg;

    [SerializeField] List<GameObject> tanks = new List<GameObject>();
    [SerializeField] GameObject ball;
    [SerializeField] GameObject goalScreen;

    [SerializeField] TextMeshProUGUI scoreTextGame;

    bool isBallActive = true;
    bool isGoalScreenActive;

    int maxGoal = 3;
    public bool isGoal { private get; set; }

    public int pointP1;
    public int pointP2;

    private void Start()
    {
        eg = GetComponent<EndGameF>();
    }

    private void Update()
    {
        if (pointP1 >= maxGoal || pointP2 >= maxGoal)
        {
            eg.isGameEnded = true;
        }
        if (isGoal)
        {
            StartCoroutine(Goal());
        }
    }

    void DisplayBall()
    {
        isBallActive = !isBallActive;
        ball.SetActive(isBallActive);
    }

    void ToggleTanks()
    {
        foreach (var tank in tanks)
        {
            TankMoveF tmf = tank.GetComponent<TankMoveF>();
            TankAttackF taf = tank.GetComponent<TankAttackF>();
            TankDashF tdf = tank.GetComponent<TankDashF>();
            TankLifeF tlf = tank.GetComponent<TankLifeF>();

            tmf.canMove = !tmf.canMove;
            tmf.lastDirectionP1 = Vector2.right;
            tmf.lastDirectionP2 = Vector2.left;
            taf.canAttack = !taf.canAttack;
            taf.currentMunitions = taf.munitionMax;
            tdf.canActiveDash = !tdf.canActiveDash;
            tlf.currentHealth = tlf.maxHealth;
        }
    }

    void TeleportTanks()
    {
        foreach (var tank in tanks)
        {
            TankLifeF tlf = tank.GetComponent<TankLifeF>();
            
            tank.transform.position = tlf.spawnPoint.transform.position;
        }
    }

    void DisplayGoalScreen()
    {
        isGoalScreenActive = !isGoalScreenActive;
        goalScreen.SetActive(isGoalScreenActive);
    }

    IEnumerator Goal()
    {
        TextMeshProUGUI scoreTextP1 = goalScreen.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(el => el.name == "ScorePlayer1");
        TextMeshProUGUI scoreTextP2 = goalScreen.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(el => el.name == "ScorePlayer2");

        isGoal = false;
        DisplayBall();
        ball.transform.position = Vector3.zero;
        ToggleTanks();
        DisplayGoalScreen();
        yield return new WaitForSeconds(0.5f);

        scoreTextP1.text = pointP1.ToString();
        scoreTextP2.text = pointP2.ToString();
        yield return new WaitForSeconds(3f);

        scoreTextGame.text = $"{pointP1}   :   {pointP2}";
        DisplayGoalScreen();
        TeleportTanks();
        DisplayBall();
        ToggleTanks();
    }
}
