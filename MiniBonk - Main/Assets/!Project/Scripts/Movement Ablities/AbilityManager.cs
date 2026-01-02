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

        private Dictionary<string, MovementAbility> _abilityDatabase = new Dictionary<string, MovementAbility>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            BuildDatabase();
        }

        #endregion
        
        #region Private Methods

        private void BuildDatabase()
        {
            foreach (var ability in allAvailAbilities)
            {
                if (ability == null) continue;
                
                _abilityDatabase.TryAdd(ability.abilityName, ability);
            }
        }
        
        private MovementAbility AbilityLookup(string abilityName)
        {
            return _abilityDatabase.GetValueOrDefault(abilityName);
        }
        #endregion

        #region Public API

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