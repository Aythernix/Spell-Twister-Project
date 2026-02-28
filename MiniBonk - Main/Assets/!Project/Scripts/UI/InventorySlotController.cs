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
    
        [Header("Properties")]
        [SerializeField] private float _scaleAmount = 1.3f;
        [SerializeField] private float _scaleSpeed = 5f;
        
        [Header("References")]
        [SerializeField] private Image _slotImage;
        [SerializeField] private ResourceBarUI _coolDownCover;
        [SerializeField] private GameObject _slot;
        [SerializeField] private Image _Keybind;

        private MovementAbility _movementAbility;
        
        private Coroutine _coolDownCoroutine;
        private Coroutine _visualiseClickCoroutine;

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
            // Displays the cooldown cover if cooldown is active
            while (_movementAbility.IsOnCooldown)
            {
                _coolDownCover.SetBarValue(_movementAbility.CooldownNormalized, 1);
                yield return null;
            }
            
        }

        private IEnumerator VisualiseClick()
        {
            Debug.Log("Visualise Click");
            // Displays the slot as active
            
            _Keybind.transform.localScale = new Vector3(1, 1, 1) * _scaleAmount;
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => !_movementAbility.IsActive);
            _Keybind.transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("Visualise Click End");
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
                return;
            }
            
            // If the cooldown coroutine is already running, stop it
            if(_coolDownCoroutine != null)
            {
                StopCoroutine(_coolDownCoroutine);
            }
            _coolDownCoroutine = StartCoroutine(CoolDownCoroutine());
            
            if(_visualiseClickCoroutine != null)
            {
                StopCoroutine(_visualiseClickCoroutine);
            }
            _visualiseClickCoroutine = StartCoroutine(VisualiseClick());
        }
        
        #endregion
    }
}
