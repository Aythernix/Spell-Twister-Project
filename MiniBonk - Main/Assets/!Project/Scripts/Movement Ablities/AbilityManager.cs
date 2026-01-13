using System;
using System.Collections.Generic;
using System.IO;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    // TODO Implement Inventory Ability Slots, Replace Debug Inputs With Proper Inputs, Add More Edge Case Checks
    // TODO Add Comments and summaries
    public class AbilityManager : PlayerSystem
    {
        public enum AbilitySlot{Slot1 = 1, Slot2 = 2, Slot3 = 3}
        
        #region Vairables

        public List<MovementAbility> allAvailAbilities;
        public List<MovementAbility> unlockedAbilities;

        [SerializeField] private MovementAbility[] equippedSlots = new MovementAbility[3];

        // Fast Lookup tables
        private readonly Dictionary<string, MovementAbility> _abilityDb = new();
        private readonly Dictionary<int, MovementAbility> _abilityIdDb = new();

        // Tracks Unlocked Ability IDs
        private readonly HashSet<int> unlockedAblityIDs = new();

        private readonly Dictionary<int, MovementAbility> _instanceIdDb = new();

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            _playerHandler.events.OnAbilityPressed += index => TriggerAbilitySlot(index);
            _playerHandler.events.OnAbilityReleased += index => FinaliseAbilitySlot(index);
            BuildDictionaries();
        }

        #endregion
        
        #region Private Methods

        // Builds the ability lookup dictionaries
        private void BuildDictionaries()
        {
            foreach (var ability in allAvailAbilities)
            {
                if (ability == null) continue;
                
                _abilityDb.TryAdd(ability.abilityName, ability);
                _abilityIdDb.TryAdd(ability.abilityID, ability);
            }
        }
        
        // Looks up the abilities
        private MovementAbility AbilityLookup(string abilityName)
        {
            return _abilityDb.GetValueOrDefault(abilityName);
        }
        private MovementAbility AbilityLookup(int abilityID)
        {
            return _abilityIdDb.GetValueOrDefault(abilityID);
        }
        // TODO Add Unequiping
        private void EquipAbility(int abilityID, int slotIndex)
        {
            
        }

        private void AutoEquipAbility(MovementAbility instance)
        {
            for (int i = 0; i < equippedSlots.Length; i++)
            {
                if (equippedSlots[i] != null && equippedSlots[i].abilityID == instance.abilityID)
                {
                    return;
                }
            }

          
            for (int i = 0; i < equippedSlots.Length; i++)
            {
                if (equippedSlots[i] == null) 
                { 
                    equippedSlots[i] = instance;
                }
                break;
            }
            
        }

        private void TriggerAbilitySlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < equippedSlots.Length)
            {
                equippedSlots[slotIndex]?.Execute();
                Debug.Log("ability triggered");
            }
            
        }

        private void FinaliseAbilitySlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < equippedSlots.Length)
            {
                equippedSlots[slotIndex]?.Finalise();
            }   
        }
        #endregion

        #region Public API

        // Allows abilities to be unlocked
        public void UnlockAbility(string abilityName)
        {
            MovementAbility ability = AbilityLookup(abilityName);

            if (ability != null && !unlockedAblityIDs.Contains(ability.abilityID))
            {
                MovementAbility instance = Instantiate(ability);
                instance.Initialize(gameObject);
                
                unlockedAbilities.Add(instance);
                unlockedAblityIDs.Add(instance.abilityID);
                _instanceIdDb.Add(instance.abilityID, instance);
                AutoEquipAbility(instance);
            }
        }
        public void UnlockAbility(int abilityID)
        {
            UnlockAbility(AbilityLookup(abilityID).abilityName);
        }

        // DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                UnlockAbility("Sprint");
                Debug.Log("unlocked");
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                foreach (var VARIABLE in unlockedAbilities)
                {
                    //VARIABLE.Execute();
                    Debug.Log("Executed");
                }
            }
            
        }

        #endregion
    } 
}