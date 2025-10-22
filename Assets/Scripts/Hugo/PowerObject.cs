using System.Diagnostics;
using UnityEngine;

public class PowerObject : MonoBehaviour
{
    enum PowerUpType
    {
        AoE,
        Power,
        Ammo
    }

    [SerializeField] private PowerUpType type;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tank"))
        {
            switch (type)
            {
                case PowerUpType.Power:
                    collision.GetComponent<TankStateB>().ChangePower();
                    break;
                case PowerUpType.AoE:
                    collision.GetComponent<TankStateB>().ChangeAoE();
                    break;
                case PowerUpType.Ammo:
                    collision.GetComponent<TankStateB>().AddAmmo();
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
