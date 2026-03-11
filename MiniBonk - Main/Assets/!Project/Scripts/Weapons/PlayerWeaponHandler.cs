using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Minibonk.Player
{
    public class PlayerWeaponHandler : PlayerSystem
    {
        // script that handles the switching of weapons

        // weapon bools

        public bool hasFireballSpell = true;
        public bool hasWaterSpell = false;
        public bool hasWindSpell = false;
        public bool hasIceSpell = false;

        public WeaponManager weaponManager;

        private void OnEnable()
        {
            _playerHandler.PlayerEvents.OnUnlockSpell += UnlockSpells;
            _playerHandler.PlayerEvents.IsSpellUnlocked += CheckIfSpellUnlocked;
        }

        private void OnDisable()
        {
            _playerHandler.PlayerEvents.OnUnlockSpell -= UnlockSpells;
            _playerHandler.PlayerEvents.IsSpellUnlocked -= CheckIfSpellUnlocked;
        }

        public void UnlockSpells(SpellTypes spellType)
        {
            switch (spellType)
            {
                case SpellTypes.FireballSpell:
                    hasFireballSpell = true;
                    break;
                case SpellTypes.WindSpell:
                    hasWindSpell = true;
                    break;
                case SpellTypes.WaterSpell:
                    hasWaterSpell = true;
                    break;
                case SpellTypes.IceSpell:
                    hasIceSpell = true;
                    break;
            }
        }

        public bool CheckIfSpellUnlocked(SpellTypes spellType)
        {
            switch (spellType)
            {
                case SpellTypes.FireballSpell:
                    return hasFireballSpell;
                    break;
                case SpellTypes.WindSpell:
                    return hasWindSpell;
                    break;
                case SpellTypes.WaterSpell:
                    return hasWaterSpell;
                    break;
                case SpellTypes.IceSpell:
                    return hasIceSpell;
                    break;
                default:
                    return false;
            }
        }

        public void EquipFireballSpell(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (hasFireballSpell)
                {
                    weaponManager.currentWeapon = 1;
                    _playerHandler.PlayerEvents.OnEquipSpell?.Invoke(SpellTypes.FireballSpell);
                }
            }
        }

        public void EquipWaterSpell(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (hasWaterSpell)
                {
                    weaponManager.currentWeapon = 2;
                    _playerHandler.PlayerEvents.OnEquipSpell?.Invoke(SpellTypes.WaterSpell);
                }
            }
        }

        public void EquipWindSpell(InputAction.CallbackContext context)
        {
            Debug.Log("EquipWindCalled");
            if (context.started)
            {
                Debug.Log("Action Started");
                if (hasWindSpell)
                {
                    Debug.Log("Equiped");
                    weaponManager.currentWeapon = 3;
                    _playerHandler.PlayerEvents.OnEquipSpell?.Invoke(SpellTypes.WindSpell);
                }
            }
        }

        public void EquipIceSpell(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (hasIceSpell)
                {
                    weaponManager.currentWeapon = 4;
                    _playerHandler.PlayerEvents.OnEquipSpell?.Invoke(SpellTypes.IceSpell);
                }
            }
        }
    }
}
