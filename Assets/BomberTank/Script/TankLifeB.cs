using UnityEngine;
using System.Collections;

public class TankLifeB : MonoBehaviour
{
    public float currentLife;
    [SerializeField] private Transform[] respawnPos;
    [SerializeField] private GameObject objectForBlink;

    [SerializeField] private float maxLife = 10;
    public bool isBlinking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                Respawn();
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
        int rand = Random.Range(0, 4);
        transform.position = respawnPos[rand].position;
    }
}
