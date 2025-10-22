using UnityEngine;

public class EndGameF : MonoBehaviour
{
    public bool isGameEnded { private get; set; }

    private void Update()
    {
        if (isGameEnded)
        {
            isGameEnded = false;
            Debug.Log("Jeux terminée !");
        }
    }
}
