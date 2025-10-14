using TMPro;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.IO;

public class TimerB : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int timer = 30;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = timer.ToString();
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
    }
}
