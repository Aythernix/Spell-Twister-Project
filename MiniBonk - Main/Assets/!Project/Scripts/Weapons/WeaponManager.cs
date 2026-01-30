using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int currentWeapon = 1;

    [SerializeField] private Weapon_FireType fireWeapon; // Type 1
    //[SerializeField] private Weapon_WaterType waterWeapon; // Type 2
    

    public void Update()
    {
        switch (currentWeapon)
        {
            case 1:
                fireWeapon.shootFireBall();
                break;
            default:
                Debug.Log("Not a weapon index!");
                break;
        }
    }
}
