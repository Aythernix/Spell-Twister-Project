using System;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public abstract class ToggleAbility : MovementAbility
    {
        [Header("Toggle Ability Stamina Settings")]
        public float staminaLossRate;
        protected bool _isActive { get; private set; }

        private void OnEnable()
        {
            _player.GetComponent<PlayerHandler>().Events.OnStaminaEmpty += OnFinalise;
        }
        private void OnDisable()
        {
            _player.GetComponent<PlayerHandler>().Events.OnStaminaEmpty -= OnFinalise;
        }

        public override void Initialize(GameObject player)
        {
            base.Initialize(player);
            
            _staminaLossRate = staminaLossRate;
        }

        protected override void OnExecute()
        {
            _isActive = true;
            OnToggle(true);
        }

        protected override void OnFinalise()
        {
            if (!_isActive) return;
            _isActive = false;
            OnToggle(false);
        }

        /// <summary>
        /// Implement Custom Ability Logic
        /// </summary>
        /// <param name="state">The current state of the toggle, <see langword="true"/> or <see langword="false"/> </param> 
        protected abstract void OnToggle(bool state);

        public override void Tick(float deltaTime)
        {
            if (_isActive)
            {
                UtiliseStamina();
            }
        }
    }
}