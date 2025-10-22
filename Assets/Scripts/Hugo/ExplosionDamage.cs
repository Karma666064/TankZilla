using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class ExplosionDamage : MonoBehaviour
{
    public BulletMoveB bulletParent;
    public bool isUsed = false;
    public bool mustBeDestroy = false;

    void OnEnable()
    {
        if (mustBeDestroy)
        {
            Destroy(gameObject, 0.25f);
            Debug.Log("Destroy Explosion");
        } else
        {
            StartCoroutine(MoveOffScreen());
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            TankLifeB tankLife = collision.GetComponent<TankLifeB>();
            tankLife.TakeDamage(1);
        }

        if (collision.CompareTag("DestructibleObject"))
        {
            Destroy(collision.gameObject);
            isUsed = false;
            //gameObject.SetActive(false);
        }
    }
    
    IEnumerator MoveOffScreen()
    {
        yield return new WaitForSeconds(0.25f);
        transform.position = new Vector3(99f, 99f, 1f);
        isUsed = false;
        gameObject.SetActive(false);
    }
}
