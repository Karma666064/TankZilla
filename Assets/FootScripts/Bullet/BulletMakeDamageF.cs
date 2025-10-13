using UnityEngine;

public class BulletMakeDamageF : MonoBehaviour
{
    public int damageToMake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            collision.gameObject.GetComponent<TankLifeF>().UpdateHealth(-damageToMake);
            Destroy(gameObject);
        }
    }
}
