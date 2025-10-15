using System.Collections;
using UnityEngine;

public class TankLifeF : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject sprite;

    [SerializeField] int maxHealth = 3;
    [SerializeField] int currentHealth;

    string player1Name = "Tank P1";
    string player2Name = "Tank P2";

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int health)
    {
        currentHealth += health;

        if (currentHealth <= 0)
        {
            if (gameObject.name == player1Name) StartCoroutine(RespawnTank());
            if (gameObject.name == player2Name) StartCoroutine(RespawnTank());
        }
    }

    public void MaxHeal()
    {
        currentHealth = maxHealth;
    }

    IEnumerator RespawnTank()
    {
        sprite.SetActive(false);
        transform.position = spawnPoint.transform.position;

        yield return new WaitForSeconds(1f);

        MaxHeal();
        sprite.SetActive(true);
    }
}
