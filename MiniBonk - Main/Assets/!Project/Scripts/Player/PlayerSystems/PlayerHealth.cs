using System;
using UnityEditor;
using UnityEngine;

namespace Minibonk.Player
{
	public class PlayerHealth : PlayerSystem
	{
		#region Variables
		//

		#endregion

		#region Unity Functions

		private void OnEnable()
		{
			// Event Subscription
			_playerHandler.Events.OnPlayerDamaged += TakeDamage;
		}

		private void OnDisable()
		{
			// Event Unsubscription
			_playerHandler.Events.OnPlayerDamaged -= TakeDamage;
		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			SetHealthToMax();
		}

		// Update is called once per frame
		void Update()
		{
			// DEBUG
			if (Input.GetKeyDown(KeyCode.F2))
			{
				TakeDamage(10f);
			}
			
			Debug.Log(_playerHandler.playerData.currentHealth);
		}

		#endregion

		#region Private Functions

		private bool HealthEmpty()
		{
			return _playerHandler.playerData.currentHealth <= 0f;
		}

		private void OnHealthEmpty()
		{
			Debug.Break();
		}

		#endregion

		#region Public API

		/// <summary>
		///  Removes health from the player
		/// </summary>
		/// <param name="damage"> The amount of health to remove from the player </param>
		public void TakeDamage(float damage)
		{
			_playerHandler.playerData.currentHealth = Mathf.Clamp(_playerHandler.playerData.currentHealth -= damage, 0, _playerHandler.playerData.maxHealth);
			if (HealthEmpty())
			{
				OnHealthEmpty();
			}
		}
		/// <summary>
		/// Sets the players health to the max health.
		/// </summary>
		public void SetHealthToMax()
		{
			_playerHandler.playerData.currentHealth = _playerHandler.playerData.maxHealth;
		}

		#endregion
	}
}