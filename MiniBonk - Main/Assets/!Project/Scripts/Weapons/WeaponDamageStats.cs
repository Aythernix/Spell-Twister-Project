using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapon/Stats")]
public class WeaponDamageStats : ScriptableObject
{
    // ----------- Weapons Stats -----------//

    public int fireBallProjectileDMG = 50;
    

    public int waterBallProjectileDMG = 60;
    

    public int windBallProjectileDMG = 25;
    

    public int iceBallProjectileDMG = 20;
    

    // --------- DOT Damage Effects ----------//

    public int frostBiteDOT = 5;
}

