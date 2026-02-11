using System;
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
            Start();
        }
        protected override void OnExecute()
        {
            ApplyImpulse();
            Finalise(); 
            UtiliseStamina();
        }

        /// <summary>
        /// Implement one time logic.
        /// </summary>
        protected abstract void ApplyImpulse();

        /// <summary>
        /// Implement any logic needed upon initialization
        /// </summary>
        protected virtual void Start()
        {
            
        }
    }
}