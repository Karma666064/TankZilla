using UnityEngine;
using System.Collections;
using System;

public class TankLifeB : MonoBehaviour
{
    public float currentLife;
    public static event Action<int, int> AddScore;
    [SerializeField] private Transform[] respawnPos;
    [SerializeField] private GameObject objectForBlink;

    [SerializeField] private float maxLife = 10;
    public bool isBlinking = false;
    private TankStateB state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = GetComponent<TankStateB>();
        currentLife = maxLife;
    }

    public void TakeDamage(float damage)
    {
        if (!isBlinking)
        {
            currentLife -= damage;
            Mathf.Clamp(currentLife, 0, maxLife);
            Debug.Log(currentLife);
            if (currentLife <= 0)
            {
                AddScore?.Invoke(state.id, 1);
                Respawn();
            }
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        isBlinking = true;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            objectForBlink.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            objectForBlink.SetActive(true);
        }

        isBlinking = false;

    }
    
    public void Respawn()
    {
        int rand = UnityEngine.Random.Range(0, 4);
        transform.position = respawnPos[rand].position;
    }
}
