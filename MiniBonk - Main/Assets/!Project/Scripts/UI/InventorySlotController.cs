using System;
using System.Collections;
using Minibonk.Management;
using Minibonk.MovementAbilities;
using UnityEngine;
using UnityEngine.UI;

namespace Minibonk.UI
{
    public class InventorySlotController : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private Image _slotImage;
        [SerializeField] private ResourceBarUI _coolDownCover;

        private MovementAbility _movementAbility;
        
        private Coroutine _coolDownCoroutine;

        #endregion
        
        #region Unity Methods

        

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

        private IEnumerator CoolDownCoroutine()
        {
            while (_movementAbility.IsOnCooldown)
            {
                _coolDownCover.SetBarValue(_movementAbility.CooldownNormalized, 1);
                yield return null;
            }
        }

        #endregion

        #region Public API

        public void SetSlot(MovementAbility movementAbility)
        {
            _movementAbility = movementAbility;
            _slotImage.sprite = movementAbility.coverIcon;
        }

        public void OnAbilityTriggered()
        {
            if (_movementAbility == null)
            {
                Debug.LogWarning($"Ability slot has triggered yet no ability has been assigned", this);
            }
            else
            {
                if(_coolDownCoroutine != null)
                {
                    StopCoroutine(_coolDownCoroutine);
                }
                _coolDownCoroutine = StartCoroutine(CoolDownCoroutine());
            }
        }
        
        #endregion
    }
}
