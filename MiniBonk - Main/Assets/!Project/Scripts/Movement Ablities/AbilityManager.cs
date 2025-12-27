
using System;
using System.Collections.Generic;
using Minibonk.Management;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public class AbilityManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private List<MovementAbility> _availableAbilities;
        [SerializeField] private List<MovementAbility> _unlockedAbilities;

        private InputManager _inputManager;
        
        #endregion

        #region Unity Methods

        private void Awake() => _inputManager = new InputManager();

        private void OnEnable()
        {
            _inputManager.Player.Enable();
            _inputManager.Player.Sprint.performed += context => TriggerAbility(a => a is SprintAbility);
        }

        private void Start()
        {
            foreach (var abilities in _unlockedAbilities)
            {
                abilities.Initialize(gameObject);
            }
        }

        #endregion

        #region Private Methods
        
        private void TriggerAbility(Predicate<MovementAbility> abilityName)
        {
            MovementAbility ability = _unlockedAbilities.Find(abilityName);
            if (ability == null)
            {
                Debug.LogWarning($"Ability: {abilityName} is not unlocked");
                return;
            }
            ability.Execute();
        }
        
        
        #endregion

        #region Public API

        public void UnlockAbility(Predicate<MovementAbility> abilityName)
        {
            MovementAbility ability = _availableAbilities.Find(abilityName);
            _unlockedAbilities.Add(ability);
            ability.Initialize(this.gameObject);
        }

        #endregion
       
    } 
}

