using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Minibonk.UI
{
	public class HealthbarUI : UISystem
	{

		#region Variable

		[SerializeField] private Image _healthSourceImage;

		private bool _updated;
		
		#endregion

		#region Unity Methods

		private void OnEnable()
		{
			_uiManager.UIEvents.updatePlayerHealthUI += CallSetHealth;

		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		protected override void Awake()
		{
			base.Awake();
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		#endregion
    
		#region Private Methods

		private void CallSetHealth(float health, float maxHealth)
		{
			StartCoroutine(SetHealthBarCoroutine(health, maxHealth));
		}
		private IEnumerator SetHealthBarCoroutine(float health, float maxHealth) {
			float targetFill = health / maxHealth;
    
			
			while (Mathf.Abs(_healthSourceImage.fillAmount - targetFill) > 0.01f) {
				_healthSourceImage.fillAmount = Mathf.Lerp(_healthSourceImage.fillAmount, targetFill, Time.deltaTime * 5f);
				yield return null; 
			}
    
			_healthSourceImage.fillAmount = targetFill; // Snap to final value
		}
		
		#endregion
    
		#region Public API
    
		//
		#endregion
    
	}

}
