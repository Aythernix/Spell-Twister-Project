using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public abstract class ImpulseAbility : MovementAbility
    {
        #region Variables
        
        [Header("Impulse Ability Stamina Settings")]
        public float staminaCost;
        
        #endregion
        public override void Initialize(GameObject player)
        {
            base.Initialize(player);
            autoFinalise = true;
            _staminaCost = staminaCost;
            Start();
        }

        #region Execution Methods
        
        protected override void OnExecute()
        {
            ApplyImpulse();
            Finalise(); 
            UtiliseStamina();
        }
        
        #endregion

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