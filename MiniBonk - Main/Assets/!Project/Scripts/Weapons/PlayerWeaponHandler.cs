using Minibonk.Player;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    // script that handles the switching of weapons

    // weapon bools

    public bool hasWaterSpell = false;
    public bool hasWindSpell = false;
    public bool hasIceSpell = false;

    public WeaponManager weaponManager;
    private void Update()
    {
        InputChecks();
    }

    void InputChecks()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            weaponManager.currentWeapon = 1;
        }
        else if (hasWaterSpell && Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            weaponManager.currentWeapon = 2;
        }
        else if (hasWindSpell && Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            weaponManager.currentWeapon = 3;     
        }
        else if (hasIceSpell && Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            weaponManager.currentWeapon = 4;
        }
    }
}
