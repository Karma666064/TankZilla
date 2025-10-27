using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class TimerB : MonoBehaviour
{
    public static event Action TimerEnd;
    private TextMeshProUGUI text;
    [SerializeField] private int timer;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = timer.ToString();

        StartTimer.StartGame += StartGameTimer;
        
    }
    
    private void StartGameTimer()
    {
        StartCoroutine(TimerGame());
    }

    IEnumerator TimerGame()
    {
        while (timer != 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            text.text = timer.ToString();
        }

        TimerEnd?.Invoke();
    }
}
