using System;
using System.Collections.Generic;
using System.IO;
using Minibonk.Management;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    // TODO Implement Inventory Ability Slots, Replace Debug Inputs With Proper Inputs, Add More Edge Case Checks
    // TODO Avoid Duplication, Add Comments and summaries
    public class AbilityManager : MonoBehaviour
    {
        #region Vairables

        public List<MovementAbility> allAvailAbilities;
        public List<MovementAbility> unlockedAbilities;

        private readonly Dictionary<string, MovementAbility> _abilityNameDictionary = new Dictionary<string, MovementAbility>();
        private readonly Dictionary<int, MovementAbility> _abilityIdDictionary = new Dictionary<int, MovementAbility>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
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
                
                _abilityNameDictionary.TryAdd(ability.abilityName, ability);
                _abilityIdDictionary.TryAdd(ability.abilityID, ability);
            }
        }
        
        // Looks up the abilities
        private MovementAbility AbilityLookup(string abilityName)
        {
            return _abilityNameDictionary.GetValueOrDefault(abilityName);
        }
        private MovementAbility AbilityLookup(int abilityID)
        {
            return _abilityNameDictionary.GetValueOrDefault(abilityID.ToString());
        }
        #endregion

        #region Public API

        // Allows abilities to be unlocked
        public void UnlockAbility(string abilityName)
        {
            MovementAbility ability = AbilityLookup(abilityName);

            if (ability != null && !unlockedAbilities.Contains(ability))
            {
                MovementAbility instance = Instantiate(ability);
                instance.Initialize(gameObject);
                unlockedAbilities.Add(instance);
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
                    VARIABLE.Execute();
                    Debug.Log("Executed");
                }
            }
            
        }

        #endregion
    } 
}