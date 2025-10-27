using UnityEngine;
using UnityEngine.UI;

public class LinkPlayButtonToManager : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GameManagerMaster.Instance.ChooseGame);
    }
}
