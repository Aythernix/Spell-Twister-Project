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
        public readonly Events PlayerEvents = new Events();
        #endregion

        #region Unity Methods
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();
            
        }

        private void OnEnable()
        {
            PlayerEvents.OnIncreaseMoney += IncreasePlayerMoney;
            PlayerEvents.OnDecreaseMoney += DecreasePlayerMoney;
            PlayerEvents.GetMoney += GetPlayerMoneyCount;
            PlayerEvents.OnIncreaseTracePoints += IncreasePlayerTracePoints;
            PlayerEvents.OnDecreaseTracePoints += DecreasePlayerTracePoints;
            PlayerEvents.GetTracePoint += GetPlayerTracePointAmount;
        }

        private void OnDisable()
        {
            PlayerEvents.OnIncreaseMoney -= IncreasePlayerMoney;
            PlayerEvents.OnDecreaseMoney -= DecreasePlayerMoney;
            PlayerEvents.GetMoney -= GetPlayerMoneyCount;
            PlayerEvents.OnIncreaseTracePoints -= IncreasePlayerTracePoints;
            PlayerEvents.OnDecreaseTracePoints -= DecreasePlayerTracePoints;
            PlayerEvents.GetTracePoint -= GetPlayerTracePointAmount;
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

        #region Private Methods

        private void IncreasePlayerMoney(int amount)
        {
            playerData.currentMoney += amount;
        }

        private void DecreasePlayerMoney(int amount)
        {
            playerData.currentMoney -= amount;
        }

        private int GetPlayerMoneyCount()
        {
            return playerData.currentMoney;
        }

        private void IncreasePlayerTracePoints(int amount)
        {
            playerData.currentTracePoints += amount;
        }

        private void DecreasePlayerTracePoints(int amount)
        {
            playerData.currentTracePoints -= amount;
        }

        private int GetPlayerTracePointAmount()
        {
            return playerData.currentTracePoints;
        }
        
        #endregion
    }
    
    /// <summary>
    /// Player Events
    /// </summary>
    public class Events
    {
        // Movement
        public Action<Vector2> OnMove;
        public Action OnJumpStarted;
        public Action OnJumpSucceeded;
        public Action OnJumpLanded;
        public Action<bool> PreventMovementInterference;
        
        // Abilities
        public Action<int> OnAbilityPressed;
        public Action<int> OnAbilityReleased;
        public Action OnDashStarted;
        public Action OnDashFinished;
        public Action OnSprintStarted;
        public Action OnSprintFinished;
       
        // Stats
        public Action<float> OnPlayerDamaged;
        public Action<float> OnStaminaDecrease;
        public Action<float> OnStaminaIncrease;
        public Action OnStaminaEmpty;
        public Action OnStaminaFull;
        public Func<bool> IsStaminaEmpty;
        public Func<bool> IsStaminaFull;
        public Func<float> GetCurrentStamina;
        public Action<int> OnIncreaseMaxStamina;
        public Action<int> OnDecreaseMaxStamina;
        public Action<int> OnIncreaseMaxHealth;
        public Action<int> OnDecreaseMaxHealth;
        public Action<int> OnIncreaseMoney;
        public Action<int> OnDecreaseMoney;
        public Func<int> GetMoney;
        public Action<int> OnIncreaseTracePoints;
        public Action<int> OnDecreaseTracePoints;
        public Func<int> GetTracePoint;

    }

}
