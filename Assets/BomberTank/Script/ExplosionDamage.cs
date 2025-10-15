using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public BulletMoveB bulletParent;
    void Start()
    {
        Destroy(gameObject, 0.25f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            TankLifeB tankLife = collision.GetComponent<TankLifeB>();  
            if (!tankLife.isBlinking)
                bulletParent.SendSignalScore();
            tankLife.TakeDamage(1);
                
        }
    }
}
