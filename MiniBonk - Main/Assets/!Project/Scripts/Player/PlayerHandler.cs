using Minibonk.Player;
using System;
using UnityEngine;

namespace Minibonk.Player
{
    // TODO Add Comments and summaries
    public class PlayerHandler : PlayerSystem
    {
        #region Variables

        public PlayerData playerData;
        public readonly Events Events = new();
        #endregion

        #region Unity Methods
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();
            
        }

        private void Start()
        {
            playerData.Reset();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        #endregion

        private void HandleTakingDamage(float damageAmount)
        {
            playerData.currentHealth -= (int)damageAmount;
            Debug.Log($"Player took {damageAmount} damage, current health: {playerData.currentHealth}");
        }
    }

    public class Events
    {
        public Action<Vector2> OnMove;
        public Action OnJumpStarted;
        public Action<int> OnAbilityPressed;
        public Action<int> OnAbilityReleased;
        public Action<bool> PreventMovementInterference;
        public Action<float> OnPlayerDamaged;
        public Action<float> OnStaminaDecrease;
        public Action<float> OnStaminaIncrease;
        public Action OnStaminaEmpty;
        public Action OnStaminaFull;
        public Func<bool> IsStaminaEmpty;
        public Func<bool> IsStaminaFull;
        public Func<float> GetCurrentStamina;
    }

}
