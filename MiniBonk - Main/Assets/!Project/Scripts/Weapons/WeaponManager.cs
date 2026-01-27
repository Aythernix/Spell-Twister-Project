using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int currentWeapon = 1;

    [SerializeField] private Weapon_FireType fireWeapon;

    private void Update()
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
