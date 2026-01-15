using Unity.VisualScripting;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public abstract class ImpulseAbility : MovementAbility
    {
        public override void Initialize(GameObject player)
        {
            base.Initialize(player);
            autoFinalise = true;
        }
        protected override void OnExecute()
        {
            ApplyImpulse();
            Finalise(); 
        }

        /// <summary>
        /// Implement one time logic.
        /// </summary>
        protected abstract void ApplyImpulse();
    }
}