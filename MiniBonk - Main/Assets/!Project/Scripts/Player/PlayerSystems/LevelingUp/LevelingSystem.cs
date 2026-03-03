using System;
using Minibonk.Management;
using Minibonk.UI;
using UnityEngine;

namespace Minibonk.Player
{

    public class LevelingSystem : PlayerSystem
    {
        #region Varibles

        [Header("XP Configuration")] 
        [SerializeField] private int _baseRequiredXp = 100;

        [SerializeField] private float _XpMultipler = 1.1f;

        [SerializeField] private int LevelUpPopUpIncrement = 1;

        // Player Level Data
        private int PlayerLevel
        {
            get => _playerHandler.playerData.level;
            set => _playerHandler.playerData.level = value;
        }

        private int PlayerXp
        {
            get => _playerHandler.playerData.xp;
            set => _playerHandler.playerData.xp = value;
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            GameManager.Instance.EventHandler.AddXp += AddXp;
            GameManager.Instance.EventHandler.IncreasePlayerLevel += OnLevelIncrement;
        }

        private void OnDisable()
        {
            GameManager.Instance.EventHandler.AddXp -= AddXp;
            GameManager.Instance.EventHandler.IncreasePlayerLevel -= OnLevelIncrement;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region Private Methods

        // Called when the XP count gets increased
        private void OnIncreaseXP(int amount)
        {
            PlayerXp += amount;
            GameManager.Instance.EventHandler.OnXpGained?.Invoke(PlayerXp);
            
            Debug.Log($" Gained: {amount} PlayerXp: {PlayerXp}");
            
            GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerXpUi?.Invoke(PlayerXp, RequiredXp(PlayerLevel));
            
            while (PlayerXp >= RequiredXp(PlayerLevel))
            {
                OnLevelIncrement();
            }
        }

        // Called when the player level increases
        private void OnLevelIncrement(int levelAmount = 1)
        {
            int cost = RequiredXp(PlayerLevel);
            PlayerXp -= cost;
            
            PlayerLevel += levelAmount;
            
            Debug.Log($"PlayerLevel: {PlayerLevel}");
            Debug.Log($"New Required XP: {RequiredXp(PlayerLevel)}");
            
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease?.Invoke(PlayerLevel);
            GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerXpUi?.Invoke(PlayerXp, RequiredXp(PlayerLevel));
            
            OpenLevelUp();
        }

        // Calculates the amount of XP the player should need to level up
        // Dependent on the players current level
        private int RequiredXp(int playerLevel)
        {
            return Mathf.RoundToInt(_baseRequiredXp * Mathf.Pow(_XpMultipler, playerLevel - 1));
        }

        private void OpenLevelUp()
        {
            if (PlayerLevel % LevelUpPopUpIncrement == 0)
            {
                Debug.Log("Level UP!");
                GameManager.Instance.uiManager.UIEvents.OnInstantiatePersistantUIScene(UISceneNames.LevelUpPopup, true);
            }
        }

        #endregion

        #region PublicAPI

        /// <summary>
        ///   Add XP to the players current XP counter
        /// </summary>
        /// <param name="amount"> the amount of XP to add</param>
        public void AddXp(int amount)
        {
            OnIncreaseXP(amount);
        }

        #endregion
    }
}

