using JetBrains.Annotations;
using UnityEngine;

public class TankStateB : MonoBehaviour
{
    public enum TankPowerBullet
    {
        levelOne,
        levelTwo,
        levelThree,
        levelFour
    }

    public int numberAmmo = 1;
    public int id;
    public int zoneAoE = 1;
    public TankPowerBullet power = TankPowerBullet.levelOne;

    public void ChangePower()
    {
        switch (power)
        {
            case TankPowerBullet.levelOne:
                power = TankPowerBullet.levelTwo;
                break;
            case TankPowerBullet.levelTwo:
                power = TankPowerBullet.levelThree;
                break;
            case TankPowerBullet.levelThree:
                power = TankPowerBullet.levelFour;
                break;
            default:
                break;
        }
    }

    public void ChangeAoE()
    {
        zoneAoE += 1;
    }

    public void AddAmmo()
    {
        numberAmmo += 1;
    }
}
