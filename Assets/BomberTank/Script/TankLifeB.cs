using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using NUnit.Framework;

public class TankLifeB : MonoBehaviour
{
    public float currentLife;
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
}
