using System;
using Minibonk.Management;
using Minibonk.UI;
using UnityEditor;
using UnityEngine;

namespace Minibonk.Player
{
	public class PlayerHealth : PlayerSystem
	{
		#region Variables
		private UIManager _uiManager;
		
		#endregion

		#region Unity Functions

		private void OnEnable()
		{
			// Event Subscription
			_playerHandler.PlayerEvents.OnPlayerDamaged += TakeDamage;
			_playerHandler.PlayerEvents.OnPlayerHeal += Heal;
			_playerHandler.PlayerEvents.OnIncreaseMaxHealth += IncreaseMaxHealth;
		}
		

		private void OnDisable()
		{
			// Event Unsubscription
			_playerHandler.PlayerEvents.OnPlayerDamaged -= TakeDamage;
			_playerHandler.PlayerEvents.OnPlayerHeal -= Heal;
			_playerHandler.PlayerEvents.OnDecreaseMaxHealth -= DecreaseMaxHealth;
		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			SetHealthToMax();

			_uiManager = GameManager.Instance.uiManager;
		}

		// Update is called once per frame
		void Update()
		{
			// DEBUG
			if (Input.GetKeyDown(KeyCode.F2))
			{
				TakeDamage(10f);
			}
			
			//Debug.Log(_playerHandler.playerData.currentHealth);
		}

		#endregion

		#region Private Functions
		
		// Checks if health is empty
		private bool HealthEmpty()
		{
			return _playerHandler.playerData.currentHealth <= 0f;
		}
		
		// Called upon death
		private void OnHealthEmpty()
		{
			Debug.Break();
		}

		private void IncreaseMaxHealth(int amount)
		{
			_playerHandler.playerData.variableMaxHealth += amount;
			
			_uiManager.UIEvents.OnUpdatePlayerHealthUI?.Invoke(_playerHandler.playerData.currentHealth,  _playerHandler.playerData.variableMaxHealth);
		}

		private void DecreaseMaxHealth(int amount)
		{
			_playerHandler.playerData.variableMaxHealth -= amount;
			
			_uiManager.UIEvents.OnUpdatePlayerHealthUI?.Invoke(_playerHandler.playerData.currentHealth,  _playerHandler.playerData.variableMaxHealth);
		}

		#endregion

		#region Public API

		/// <summary>
		///  Removes health from the player
		/// </summary>
		/// <param name="damage"> The amount of health to remove from the player </param>
		public void TakeDamage(float damage)
		{
			_playerHandler.playerData.currentHealth = Mathf.Clamp(_playerHandler.playerData.currentHealth -= damage, 0, _playerHandler.playerData.variableMaxHealth);
			if (HealthEmpty())
			{
				OnHealthEmpty();
			}
			
			_uiManager.UIEvents.OnUpdatePlayerHealthUI?.Invoke(_playerHandler.playerData.currentHealth,  _playerHandler.playerData.variableMaxHealth);
			
		}

		public void Heal(float amount)
		{
			_playerHandler.playerData.currentHealth = Mathf.Clamp(_playerHandler.playerData.currentHealth += amount, 0, _playerHandler.playerData.variableMaxHealth);
			
			_uiManager.UIEvents.OnUpdatePlayerHealthUI?.Invoke(_playerHandler.playerData.currentHealth,  _playerHandler.playerData.variableMaxHealth);
		}
		/// <summary>
		/// Sets the players health to the max health.
		/// </summary>
		public void SetHealthToMax()
		{
			_playerHandler.playerData.currentHealth = _playerHandler.playerData.variableMaxHealth;
		}

		#endregion
	}
}