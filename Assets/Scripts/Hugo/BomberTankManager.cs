using TMPro;
using UnityEngine;

public class BomberTankManager : MonoBehaviour
{
    [SerializeField] private ScoreB score;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private GameObject victoryPanel;
    void Start()
    {
        TimerB.TimerEnd += EndGame;
    }

    private void EndGame()
    {
        victoryPanel.SetActive(true);
        if (score.scoreP1 > score.scoreP2)
        {
            GameManagerMaster.Instance.pointPlayerOne += 1;
            victoryText.text = GameManagerMaster.Instance.playerOneName +  " WIN!";
        }
        else if (score.scoreP1 < score.scoreP2)
        {
            GameManagerMaster.Instance.pointPlayerTwo += 1;
            victoryText.text = GameManagerMaster.Instance.playerTwoName + " WIN!";
        }
    }
}
