using System;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public abstract class ToggleAbility : MovementAbility
    {
        #region Variables
        
        [Header("Toggle Ability Stamina Settings")]
        public float staminaLossRate;
        protected bool _isActive { get; private set; }
        
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _player.GetComponent<PlayerHandler>().PlayerEvents.OnStaminaEmpty += OnFinalise;
        }
        private void OnDisable()
        {
            _player.GetComponent<PlayerHandler>().PlayerEvents.OnStaminaEmpty -= OnFinalise;
        }
        
        
        #endregion

        public override void Initialize(GameObject player)
        {
            base.Initialize(player);
            
            _staminaLossRate = staminaLossRate;
        }

        #region Execution Methods
        /// <summary>
        /// Runs when the player activates the ability key
        /// </summary>
        protected override void OnExecute()
        {
            _isActive = true;
            OnToggle(true);
        }
        
        /// <summary>
        /// Runs when the player releases the ability key
        /// </summary>
        protected override void OnFinalise()
        {
            if (!_isActive) return;
            _isActive = false;
        }

        #endregion

        /// <summary>
        /// Implement Custom Ability Logic
        /// </summary>
        /// <param name="state">The current state of the toggle, <see langword="true"/> or <see langword="false"/> </param> 
        protected abstract void OnToggle(bool state);
        
        /// <summary>
        /// Runs an Update loop in the SO
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Tick(float deltaTime)
        {
            if (_isActive)
            {
                UtiliseStamina();
            }
        }
    }
}