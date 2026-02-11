using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapon/Stats")]
public class WeaponDamageStats : ScriptableObject
{
    // ----------- Weapons Stats -----------//

    public int fireBallProjectileDMG = 50;
    public int fireBallProjectileDMG_LVL2 = 75;

    public int waterBallProjectileDMG = 60;
    public int waterBallProjectileDMG_LVL2 = 85;

    public int windBallProjectileDMG = 25;
    public int windBallProjectileDMG_LVL2 = 30;

    public int iceBallProjectileDMG = 20;
    public int iceBallProjectileDMG_LVL2 = 25;

    // --------- DOT Damage Effects ----------//

    public int frostBiteDOT = 5;
}

