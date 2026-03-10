using System;
using Minibonk.Management;
using UnityEngine;

public enum SpellTypes
{
    FireballSpell,
    WaterSpell,
    WindSpell,
    IceSpell,
}

public class WeaponManager : MonoBehaviour
{
    // determins what weapon is currently active
    public int currentWeapon = 1;

    // Weapon scripts

    [SerializeField] private Weapon_FireType fireWeapon; // Type 1
    [SerializeField] private Weapon_WaterType waterWeapon; // Type 2
    [SerializeField] private Weapon_WindType windWeapon; // Type 3
    [SerializeField] private Weapon_IceType iceWeapon; // Type 4

    private void OnEnable()
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.IsSpellEquipped += GetCurrentWeapon;
    }

    private void OnDisable()
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.IsSpellEquipped -= GetCurrentWeapon;
    }

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

    private bool GetCurrentWeapon(SpellTypes spellType)
    {
        return (int)spellType + 1 == currentWeapon;
    }
}
