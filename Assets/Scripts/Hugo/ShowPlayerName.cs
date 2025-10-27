using TMPro;
using UnityEngine;

public class ShowPlayerName : MonoBehaviour
{
    [SerializeField] private int id = 1;
    void Start()
    {
        if (id == 1)
            GetComponent<TextMeshProUGUI>().text = GameManagerMaster.Instance.playerOneName;
        else
            GetComponent<TextMeshProUGUI>().text = GameManagerMaster.Instance.playerTwoName;
    }
}
