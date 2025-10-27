using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class StartTimer : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    public static event Action StartGame;
    private TextMeshProUGUI textMP;
    private int timer = 3;
    void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();
        textMP.text = timer.ToString();
        StartCoroutine(WaitStartTimer());
    }

    IEnumerator WaitStartTimer()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            if (timer != 0)
                textMP.text = timer.ToString();

            else
                textMP.text = "GO!!";
        }

        yield return new WaitForSeconds(1f);
        StartGame?.Invoke();
        startPanel.SetActive(false);
    }
}
