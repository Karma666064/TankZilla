using UnityEngine;

public class BallDetectorF : MonoBehaviour
{
    [SerializeField] GameObject ball;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("But!!!!!!!!!");
        }
    }
}
