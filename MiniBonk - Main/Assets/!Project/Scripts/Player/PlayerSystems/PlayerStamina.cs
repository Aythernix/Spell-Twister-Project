using System;
using Minibonk.Management;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.Player
{
	public class PlayerStamina : PlayerSystem
	{
		#region Varibles
		
		[Header("Player Stamina")]
		[SerializeField] private float _staminaRecoveryPerSecond;
		[SerializeField] private float _staminaRecoveryCooldown;
		[SerializeField] private float _timeSinceStaminaLost;

		public bool isEmpty = false;

		#endregion
		
		#region Unity Methods

		private void OnEnable()
		{
			_playerHandler.PlayerEvents.OnStaminaDecrease += DecrementPlayerStamina;
			_playerHandler.PlayerEvents.OnStaminaIncrease += IncrementPlayerStamina;
			_playerHandler.PlayerEvents.IsStaminaEmpty = () => isEmpty;
			_playerHandler.PlayerEvents.IsStaminaFull = () => _playerHandler.playerData.currentStamina >= _playerHandler.playerData.maxStamina;
			_playerHandler.PlayerEvents.GetCurrentStamina = () => _playerHandler.playerData.currentStamina;
		}

		private void OnDisable()
		{
			_playerHandler.PlayerEvents.OnStaminaDecrease -= DecrementPlayerStamina;
			_playerHandler.PlayerEvents.OnStaminaIncrease -= IncrementPlayerStamina;
		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
			StaminaCooldown();
			
			Debug.Log($"Stamina: {_playerHandler.playerData.currentStamina}/{_playerHandler.playerData.maxStamina}");
			
		}
		#endregion

		#region Private Methods
		
		/// <summary>
		/// Decreases the players stamina
		/// </summary>
		/// <param name="amount"> amount to decrease by </param>
		private void DecrementPlayerStamina(float amount)
		{
			_playerHandler.playerData.currentStamina -= amount;
			_timeSinceStaminaLost = Time.time;
			
			if (_playerHandler.playerData.currentStamina <= 0)
			{
				OnStaminaEmpty();
			}
			
			_playerHandler.playerData.currentStamina = Mathf.Clamp(_playerHandler.playerData.currentStamina, 0 , _playerHandler.playerData.maxStamina);
			
			Debug.Log($"Stamina Decreased: {amount}");
			
			UpdateUI();
		}
		
		/// <summary>
		/// Increases the player stamina
		/// </summary>
		/// <param name="amount"> amount to increase by </param>
		private void IncrementPlayerStamina(float amount)
		{
			_playerHandler.playerData.currentStamina += amount;
			
			_playerHandler.playerData.currentStamina = Mathf.Clamp(_playerHandler.playerData.currentStamina, 0 , _playerHandler.playerData.maxStamina);
			
			UpdateUI();
		}
		
		// Regenerates the players stamina after a cooldown
		private void RecoverPlayerStamina(float amount)
		{
			IncrementPlayerStamina(amount);
		}

		// Calls stamina recovery after a cooldown if stamina isn't full
		private void StaminaCooldown()
		{
			if (_playerHandler.playerData.currentStamina <= _playerHandler.playerData.maxStamina)
			{
				float staminaPerSecond = _staminaRecoveryPerSecond * Time.deltaTime;
				if (Time.time >= _timeSinceStaminaLost + _staminaRecoveryCooldown)
				{
					RecoverPlayerStamina(staminaPerSecond);
				}
			}
		}
		
		// Fires an event if the stamina reaches zero
		private void OnStaminaEmpty()
		{
			isEmpty = true;
			_playerHandler.PlayerEvents.OnStaminaEmpty?.Invoke();
		}

		// Updates the UI every change
		private void UpdateUI()
		{
			GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerStaminaUI?.Invoke(_playerHandler.playerData.currentStamina, _playerHandler.playerData.maxStamina);
		}

		#endregion
	}
}

