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
                _reward = (int)(_unlockableData.reward * GameManager.Instance.currentLevelManager.playerHandler.playerData.level * _unlockableData.levelMultiplier);
                _animator.runtimeAnimatorController = _unlockableData.animatorController;
                _rewardHeading.text = _reward + " " + _unlockableData.currency;
                _description.text = _unlockableData.description;
            }
        }
    
        public void ExecuteReward()
        {
            if (!_unlockableData.StatUpgrade)
            {
                if (_unlockableData.maxHealthIncrease)
                {
                    _unlockableData.IncreaseMaxHealth(_reward);
                }
                if (_unlockableData.maxStaminaIncrease)
                {
                    _unlockableData.IncreaseMaxStamina(_reward);
                }
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