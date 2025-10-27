using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private CheckPseudo playerOnePseudo;
    [SerializeField] private CheckPseudo playerTwoPseudo;

    [SerializeField] private Button start;

    // Update is called once per frame
    void Update()
    {
        if (playerOnePseudo.isValided && playerTwoPseudo.isValided)
        {
            start.interactable = true;
        } else
        {
            start.interactable = false;
        }
    }
}
