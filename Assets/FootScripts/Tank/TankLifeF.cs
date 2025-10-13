using UnityEngine;

public class TankLifeF : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int health)
    {
        currentHealth += health;

        if (currentHealth <= 0)
        {
            Debug.Log("Player Dead!");
        }
    }

    public void MaxHeal()
    {
        currentHealth = maxHealth;
    }
}
