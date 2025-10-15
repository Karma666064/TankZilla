using UnityEngine;

public class BulletDestroyOnWallB : MonoBehaviour
{
    [SerializeField] private BulletMoveB parent;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            parent.DestroyOnContact(0f);
        }
    }
}
