using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int currentWeapon = 2;

    [SerializeField] private Weapon_FireType fireWeapon; // Type 1
    [SerializeField] private Weapon_WaterType waterWeapon; // Type 2
    [SerializeField] private Weapon_WindType windWeapon; // Type 3
    [SerializeField] private Weapon_IceType iceWeapon; // Type 4
    

    public void Update()
    {
        switch (currentWeapon)
        {
            case 1:
                fireWeapon.shootFireBall();
                break;
            case 2:
                waterWeapon.ShootWaterPulse();
                break;
            case 3:
                windWeapon.ShootWindBlade();
                break;
            case 4:
                iceWeapon.ShootIceProjectile();
                break;
            default:
                Debug.Log("Not a weapon index!");
                break;
        }
    }
}
