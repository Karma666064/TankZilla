using TMPro;
using UnityEngine;

public class ScoreB : MonoBehaviour
{
    private int scoreP1 = 0;
    private int scoreP2 = 0;
    [SerializeField] private TextMeshProUGUI textScoreP1;
    [SerializeField] private TextMeshProUGUI textScoreP2;
    void Start()
    {
        textScoreP1.text = "Score P1: " + scoreP1.ToString();
        textScoreP2.text = "Score P2: " + scoreP2.ToString();
        BulletMoveB.AddScore += UpdateScore;
    }

    void UpdateScore(int id, int _score)
    {
        switch (id)
        {
            case 1:
                scoreP1 += _score;
                textScoreP1.text = "Score P1: " + scoreP1.ToString();
                break;
            case 2:
                scoreP2 += _score;
                textScoreP2.text = "Score P2: " + scoreP2.ToString();
                break;
            default:
                break;
        }
    }
}
