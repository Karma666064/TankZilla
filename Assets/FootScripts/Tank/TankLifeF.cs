using System.Collections;
using UnityEngine;

public class TankLifeF : MonoBehaviour
{
    public GameObject spawnPoint;
    [SerializeField] GameObject sprite;

    public int maxHealth = 3;
    public int currentHealth ;

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
