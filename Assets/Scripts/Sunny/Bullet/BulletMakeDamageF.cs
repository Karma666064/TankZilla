using UnityEngine;

public class BulletMakeDamageF : MonoBehaviour
{
    public int damageToMake;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            collision.gameObject.GetComponent<TankLifeF>().UpdateHealth(-damageToMake);
            Destroy(gameObject);
        }
    }
}
