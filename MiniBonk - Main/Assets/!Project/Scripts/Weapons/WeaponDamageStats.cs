using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapon/Stats")]
public class WeaponDamageStats : ScriptableObject
{
    public int fireBallProjectileDMG = 50;
    public int fireBallProjectileDMG_LVL2 = 75;

    public int waterBallProjectileDMG = 60;
}

