using System;
using System.Collections;
using Minibonk.Management;
using Minibonk.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Minibonk.Player
{
	public class PlayerHealth : PlayerSystem
	{
		#region Variables

		[SerializeField] private float _allowDamageTimer = 0.5f;
		[SerializeField] private float _lowHealthThreshold = 20f;
		[SerializeField] private Color _lowHealthColor = Color.red;
		[SerializeField] private float _vignetteTransitionDuration = 1f;
		private bool _isLowHealth = false;
		
		private UIManager _uiManager;
		
		private bool _allowDamage = true;
		private Coroutine _takeDamageCoroutine;
		private Coroutine _vignetteTransitionCoroutine;
		
		private Vignette _vignette;
		
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

			if (GameManager.Instance.currentLevelManager.postProcessVolume.profile.TryGet<Vignette>(out var vignette))
			{
				_vignette = vignette;
			}
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
			GameManager.Instance.uiManager.UIEvents.SwapActiveUIScene?.Invoke(UISceneNames.DeathScreen);
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
		
		private IEnumerator AllowDamageTimer()
		{
			yield return new WaitForSeconds(_allowDamageTimer);
			_allowDamage = true;
		}
		
		private IEnumerator SmoothVignetteTransition(Color startColour, Color endColour)
		{
			float elapsedTime = 0f;
			
			while (elapsedTime < _vignetteTransitionDuration)
			{
				elapsedTime += Time.deltaTime;
				float t = Mathf.Clamp01(elapsedTime / _vignetteTransitionDuration);
				_vignette.color.Override(Color.Lerp(startColour, endColour, t));
				yield return null;
			}
			
			_vignette.color.Override(endColour);
		}

		#endregion

		#region Public API

		/// <summary>
		///  Removes health from the player
		/// </summary>
		/// <param name="damage"> The amount of health to remove from the player </param>
		public void TakeDamage(float damage)
		{
			if (!_allowDamage) return;
			
			_allowDamage = false;
			_playerHandler.playerData.currentHealth = Mathf.Clamp(_playerHandler.playerData.currentHealth -= damage, 0, _playerHandler.playerData.variableMaxHealth);
			if (HealthEmpty())
			{
				OnHealthEmpty();
			}
			
			_uiManager.UIEvents.OnUpdatePlayerHealthUI?.Invoke(_playerHandler.playerData.currentHealth,  _playerHandler.playerData.variableMaxHealth);

			if (_playerHandler.playerData.currentHealth <= _lowHealthThreshold && !_isLowHealth)
			{
				_isLowHealth = true;
				if (_vignetteTransitionCoroutine != null)
				{
					StopCoroutine(_vignetteTransitionCoroutine);
				}
				_vignetteTransitionCoroutine = StartCoroutine(SmoothVignetteTransition(Color.black, _lowHealthColor));
			}
			
			if (_takeDamageCoroutine != null)
			{
				StopCoroutine(_takeDamageCoroutine);
			}
			_takeDamageCoroutine = StartCoroutine(AllowDamageTimer());
			
		}

		public void Heal(float amount)
		{
			_playerHandler.playerData.currentHealth = Mathf.Clamp(_playerHandler.playerData.currentHealth += amount, 0, _playerHandler.playerData.variableMaxHealth);
			
			_uiManager.UIEvents.OnUpdatePlayerHealthUI?.Invoke(_playerHandler.playerData.currentHealth,  _playerHandler.playerData.variableMaxHealth);
			
			if (_playerHandler.playerData.currentHealth > _lowHealthThreshold && _isLowHealth)
			{
				_isLowHealth = false;
				if (_vignetteTransitionCoroutine != null)
				{
					StopCoroutine(_vignetteTransitionCoroutine);
				}
				_vignetteTransitionCoroutine = StartCoroutine(SmoothVignetteTransition(_lowHealthColor, Color.black));
			}
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