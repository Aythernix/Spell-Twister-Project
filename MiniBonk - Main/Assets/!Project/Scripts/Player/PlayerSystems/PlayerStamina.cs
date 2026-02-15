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
			_playerHandler.Events.OnStaminaDecrease += DecrementPlayerStamina;
			_playerHandler.Events.OnStaminaIncrease += IncrementPlayerStamina;
			_playerHandler.Events.IsStaminaEmpty = () => isEmpty;
			_playerHandler.Events.IsStaminaFull = () => _playerHandler.playerData.currentStamina >= _playerHandler.playerData.maxStamina;
			_playerHandler.Events.GetCurrentStamina = () => _playerHandler.playerData.currentStamina;
		}

		private void OnDisable()
		{
			_playerHandler.Events.OnStaminaDecrease -= DecrementPlayerStamina;
			_playerHandler.Events.OnStaminaIncrease -= IncrementPlayerStamina;
		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
			if (_playerHandler.playerData.currentStamina <= _playerHandler.playerData.maxStamina)
			{
				float staminaPerSecond = _staminaRecoveryPerSecond * Time.deltaTime;
				if (Time.time >= _timeSinceStaminaLost + _staminaRecoveryCooldown)
				{
					RecoverPlayerStamina(staminaPerSecond);
				}
			}
			
			Debug.Log($"Stamina: {_playerHandler.playerData.currentStamina}/{_playerHandler.playerData.maxStamina}");
			
		}
		#endregion

		#region Private Methods

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

		private void IncrementPlayerStamina(float amount)
		{
			_playerHandler.playerData.currentStamina += amount;
			
			_playerHandler.playerData.currentStamina = Mathf.Clamp(_playerHandler.playerData.currentStamina, 0 , _playerHandler.playerData.maxStamina);
			
			UpdateUI();
		}

		private void RecoverPlayerStamina(float amount)
		{
			IncrementPlayerStamina(amount);
		}

		private void OnStaminaEmpty()
		{
			isEmpty = true;
			_playerHandler.Events.OnStaminaEmpty?.Invoke();
		}

		private void UpdateUI()
		{
			GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerStaminaUI?.Invoke(_playerHandler.playerData.currentStamina, _playerHandler.playerData.maxStamina);
		}

		#endregion
	}
}

