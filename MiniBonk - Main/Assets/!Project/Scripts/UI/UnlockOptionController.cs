using Minibonk.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniBonk.UI
{
   

    public class UnlockOptionController : MonoBehaviour
    {
        #region Variable

        [SerializeField] private PopUpUnlockable _unlockableData;

        [Header("Unlockable Data")]
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _rewardHeading;
        [SerializeField] private TextMeshProUGUI _description;

        private int _reward;

        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
    
        public void SetUnlockableData(PopUpUnlockable unlockableData)
        {
            _unlockableData = unlockableData;
        
            if (_unlockableData != null)
            {
                _reward = (int)(_unlockableData.reward * Mathf.Pow(_unlockableData.levelMultiplier, GameManager.Instance.currentLevelManager.playerHandler.playerData.level - 1));
                _animator.runtimeAnimatorController = _unlockableData.animatorController;
                _rewardHeading.text = _reward + " " + _unlockableData.currency;
                _description.text = _unlockableData.description;
            }
        }
    
        public void ExecuteReward()
        {
            Debug.Log($"Executing reward with amount: {_reward}");
           
            if (_unlockableData.maxHealthIncrease)
            {
                Debug.Log($"Increasing max health by {_reward}");
                _unlockableData.IncreaseMaxHealth(_reward);
            }
            if (_unlockableData.maxStaminaIncrease)
            {
                Debug.Log($"Increasing max stamina by {_reward}");
                _unlockableData.IncreaseMaxStamina(_reward);
            }
            
        
            if (_unlockableData.moneyIncrease)
            {
                _unlockableData.IncreaseMoney(_reward);
            }
            if (_unlockableData.traceIncrease)
            {
                _unlockableData.IncreaseTrace(_reward);
            }
        }
    }

}
