using Minibonk.MovementAbilities;
using Minibonk.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Minibonk.MovementAbilities
{
    public abstract class ToggleAbility : MovementAbility
    {
        [SerializeField] protected float _multiplier = 1.5f;
        protected bool _isActive;

        public override void Execute()
        {
            // Only allow starting if not on cooldown
            if (IsOnCooldown) return;

            _isActive = true;
            Toggle(true);
        }

        public override void Finalise()
        {
            if (!_isActive) return;

            _isActive = false;
            Toggle(false);

            // Start the cooldown only when the ability is finished
            StartCooldown();
        }

        protected abstract void Toggle(bool status);
    }
}

