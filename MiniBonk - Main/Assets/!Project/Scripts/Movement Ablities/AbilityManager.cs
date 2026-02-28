using System;
using System.Collections.Generic;
using System.IO;
using Minibonk.Management;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    // TODO Implement Inventory Ability Slots, Replace Debug Inputs With Proper Inputs, Add More Edge Case Checks
    // TODO Add Comments and summaries
    public class AbilityManager : PlayerSystem
    {
        
        #region Vairables

        public List<MovementAbility> allAvailAbilities;
        public List<MovementAbility> unlockedAbilities;

        [SerializeField] private MovementAbility[] equippedSlots = new MovementAbility[3];

        // Fast Lookup tables
        public readonly Dictionary<string, MovementAbility> _abilityDb = new();
        public readonly Dictionary<int, MovementAbility> _abilityIdDb = new();

        // Tracks Unlocked Ability IDs
        public readonly HashSet<int> unlockedAblityIDs = new();

        public readonly Dictionary<int, MovementAbility> _instanceIdDb = new();

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            BuildDictionaries();
        }

        private void OnEnable()
        {
            _playerHandler.PlayerEvents.OnAbilityPressed += TriggerAbilitySlot;
            _playerHandler.PlayerEvents.OnAbilityReleased += FinaliseAbilitySlot;
           
        }
        private void OnDisable()
        {
            _playerHandler.PlayerEvents.OnAbilityPressed -= TriggerAbilitySlot;
            _playerHandler.PlayerEvents.OnAbilityReleased -= FinaliseAbilitySlot;
        }
        
        // DEBUG
        private void Update()
        {
            foreach (MovementAbility ability in unlockedAbilities)
            {
                ability.Tick(Time.time);
            }
            
            if (Input.GetKeyDown(KeyCode.F1))
            {
                UnlockAbility("Sprint");
                UnlockAbility("Dash");
                Debug.Log("unlocked");
            }
            
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
            _instanceIdDb.TryGetValue(abilityID, out var instance);
            equippedSlots[slotIndex] = instance;
            GameManager.Instance.EventHandler.OnAbilityEquipped?.Invoke(slotIndex, instance);
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
                    EquipAbility(instance.abilityID, i);
                    break;
                }
                
            }
            
        }

        private void TriggerAbilitySlot(int slotIndex)
        {
            if (equippedSlots[slotIndex] == null) return;
            if (slotIndex >= 0 && slotIndex < equippedSlots.Length)
            {
                equippedSlots[slotIndex]?.Execute();
                Debug.Log("ability triggered");
                GameManager.Instance.EventHandler.OnAbilitySlotTriggered?.Invoke(slotIndex);
            }
            
        }

        private void FinaliseAbilitySlot(int slotIndex)
        {
            if (equippedSlots[slotIndex] == null) return;
            if (slotIndex >= 0 && slotIndex < equippedSlots.Length)
            {
                Debug.Log(equippedSlots[slotIndex].abilityName);
                if (!equippedSlots[slotIndex].autoFinalise)
                {
                    equippedSlots[slotIndex]?.Finalise();
                }
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

        #endregion
    } 
}