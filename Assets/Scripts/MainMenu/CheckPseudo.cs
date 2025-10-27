using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CheckPseudo : MonoBehaviour
{
    private TextMeshProUGUI textMP;
    [SerializeField] private TMP_InputField input;
    public bool isValided = false;

    public int id = 1;

    void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (textMP.text.Length != 1)
        {
            Check();
        }
    }
    
    private void Check()
    {
        if (textMP.text.Length > 3 && textMP.text.Length <= 17)
        {
            isValided = true;
        }
        else
        {
            isValided = false;
        }

        if (textMP.text.Length >= 17)
        {
            input.text = textMP.text.Substring(0, 16);
            textMP.text = textMP.text.Substring(0, 16);
        }

        if (id == 1)
            GameManagerMaster.Instance.playerOneName = input.text;
        else
            GameManagerMaster.Instance.playerTwoName = input.text;
    }
}
