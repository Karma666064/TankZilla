using System.Collections.Generic;
using UnityEngine;

public class StartGameF : MonoBehaviour
{
    TimerF timer;

    [SerializeField] List<GameObject> tanks = new List<GameObject>();
    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();

    private void Start()
    {
        timer = GetComponent<TimerF>();

        GameObject tankP1 = tanks[0];
        GameObject tankP2 = tanks[1];

        tankP1.transform.position = spawnPoints[0].transform.position;
        tankP2.transform.position = spawnPoints[1].transform.position;

        timer.StartTimer();
    }
}
