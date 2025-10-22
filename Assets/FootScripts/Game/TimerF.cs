using TMPro;
using UnityEngine;

public class TimerF : MonoBehaviour
{
    EndGameF eg;

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] float duration = 60f;
    [HideInInspector] public float timer;
    bool isRunning = true;

    private void Start()
    {
        eg = GetComponent<EndGameF>();
    }

    void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        SetTextTimer(timer);
    }

    public void StartTimer()
    {
        timer = duration;
        isRunning = true;
    }

    void SetTextTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00} : {seconds:00}";
    }

    private void OnTimerEnd()
    {
        eg.isGameEnded = true;
    }
}
