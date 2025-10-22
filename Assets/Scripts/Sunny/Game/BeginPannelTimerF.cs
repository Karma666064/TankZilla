using System.Collections;
using TMPro;
using UnityEngine;

public class BeginPannelTimerF : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTimer;

    int timer = 5;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (timer > -1)
        {
            textTimer.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }
}
