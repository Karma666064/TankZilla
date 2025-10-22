using UnityEngine;

public class BallDetectorF : MonoBehaviour
{
    [SerializeField] GameObject manager;
    GoalF goal;

    public bool isGoalP1;

    private void Start()
    {
        goal = manager.GetComponent<GoalF>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            goal.isGoal = true;

            if (isGoalP1) goal.pointP1++;
            else goal.pointP2++;
        }
    }
}
