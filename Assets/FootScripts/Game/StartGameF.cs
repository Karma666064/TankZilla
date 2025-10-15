using System.Collections.Generic;
using UnityEngine;

public class StartGameF : MonoBehaviour
{
    [SerializeField] List<GameObject> tanks = new List<GameObject>();
    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();

    private void Start()
    {
        // Place the tanks on good point
        tanks[0].transform.position = spawnPoints[0].transform.position;
        tanks[1].transform.position = spawnPoints[1].transform.position;


    }
}
