using System;
using System.Collections.Generic;
using Minibonk.Management;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    
    //TODO REWORK THIS SCRIPT
    public class AbilityManager : MonoBehaviour
    {
        #region Variables
        
        [Header("Ability Setup")]
        // We keep the list for the Inspector, but we move it to a Dictionary for speed
        [SerializeField] private List<MovementAbility> _startingAbilities;

        // Fast lookup: Key is the Type (e.g., SprintAbility), Value is the specific instance
        private Dictionary<Type, MovementAbility> _abilityCache = new Dictionary<Type, MovementAbility>();

        private InputManager _inputManager;
        
        #endregion

        #region Unity Methods

        private void Awake() => _inputManager = new InputManager();

        private void OnEnable()
        {
            _inputManager.Player.Enable();

            // OPTIMIZED: No searching. We ask the dictionary for the specific type.
            _inputManager.Player.Sprint.performed += _ => GetAbility<ToggleAbility>()?.Execute();
            _inputManager.Player.Sprint.canceled += _ => GetAbility<ToggleAbility>()?.Finalise();
            
            // Example for a Dash:
            // _inputManager.Player.Dash.performed += _ => GetAbility<DashAbility>()?.Execute();
        }
        
        private void Start()
        {
            // Initialize starting abilities
            foreach (var abilityAsset in _startingAbilities)
            {
                UnlockAbility(abilityAsset);
            }
        }

        #endregion

        #region Private Methods
        
        // This is O(1) complexity—the fastest possible way to get an ability
        private T GetAbility<T>() where T : MovementAbility
        {
            if (_abilityCache.TryGetValue(typeof(T), out MovementAbility ability))
            {
                return (T)ability;
            }
            return null;
        }
        
        #endregion

        #region Public API

        public void UnlockAbility(MovementAbility abilityAsset)
        {
            if (abilityAsset == null) return;

            Type type = abilityAsset.GetType();

            if (!_abilityCache.ContainsKey(type))
            {
                // 1. Create a unique instance (Safe for team-play/cooldowns)
                MovementAbility instance = Instantiate(abilityAsset);
                
                // 2. Setup
                instance.Initialize(this.gameObject);
                
                // 3. Store in Dictionary
                _abilityCache.Add(type, instance);
            }
        }

        #endregion
    } 
}