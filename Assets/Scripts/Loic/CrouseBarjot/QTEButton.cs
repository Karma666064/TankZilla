using UnityEngine;
using TMPro;
using System.Collections;

public class QTEButton : MonoBehaviour
{
    public enum ButtonType { A, B, X, Y }
    public ButtonType correctButton;
    public TextMeshPro buttonText;
    public GameObject visualIndicator;
    public int playerNumber = 1;
   
    private bool isActivated = false;
    private bool isInitialized = false;

    void Start()
    {
        if (!isInitialized)
        {
            InitializeQTE();
        }
        HideQTE();
    }

    void InitializeQTE()
    {
        if (isInitialized) return;
    
        correctButton = (ButtonType)Random.Range(0, 4);
       
        if (visualIndicator != null)
        {
            SpriteRenderer sr = visualIndicator.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.white;
            }
        }
        
        UpdateButtonText();
        
        isInitialized = true;
        
        Debug.Log($"QTE initialisé pour joueur {playerNumber}, bouton: {correctButton}, texte: {buttonText?.text}");
    }
   
    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            if (playerNumber == 1)
            {
                switch (correctButton)
                {
                    case ButtonType.A:
                        buttonText.text = "S";
                        break;
                    case ButtonType.B:
                        buttonText.text = "D";
                        break;
                    case ButtonType.X:
                        buttonText.text = "Q";
                        break;
                    case ButtonType.Y:
                        buttonText.text = "Z";
                        break;
                }
            }
            else
            {
                buttonText.text = correctButton.ToString();
            }
        }
    }
   
    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
        
        InitializeQTE();
       
        Debug.Log($"QTE assigné au joueur {playerNumber}, touche affichée: {buttonText?.text}");
    }
   
    public void ShowQTE()
    {
        if (visualIndicator != null)
        {
            visualIndicator.SetActive(true);
        }
       
        if (buttonText != null)
        {
            buttonText.gameObject.SetActive(true);
        }
    }
   
    public void HideQTE()
    {
        if (visualIndicator != null)
        {
            visualIndicator.SetActive(false);
        }
       
        if (buttonText != null)
        {
            buttonText.gameObject.SetActive(false);
        }
    }
   
    public bool CheckInput(ButtonType pressedButton)
    {
        if (isActivated) return true;
       
        isActivated = true;
       
        if (pressedButton == correctButton)
        {
            return true;
        }
        else
        {
            StartCoroutine(ErrorEffect());
            return false;
        }
    }
   
    IEnumerator ErrorEffect()
    {
        if (visualIndicator != null)
        {
            SpriteRenderer sr = visualIndicator.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color originalColor = sr.color;
               
                for (int i = 0; i < 3; i++)
                {
                    sr.color = Color.black;
                    yield return new WaitForSeconds(0.1f);
                    sr.color = Color.red;
                    yield return new WaitForSeconds(0.1f);
                }
               
                sr.color = originalColor;
            }
        }
        
        isActivated = false;
    }
}