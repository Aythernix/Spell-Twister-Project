using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Minibonk.UI
{
	public class ResourceBarUI : UISystem
	{

		#region Variable

		[SerializeField] private Image _healthSourceImage;
		[SerializeField] private float _fillRate;
		
		private Coroutine _currentBarCoroutine;
		
		#endregion

		#region Unity Methods

		private void OnEnable()
		{
			_uiManager.UIEvents.OnUpdatePlayerHealthUI += SetBarValue;
		}

		private void OnDisable()
		{
			_uiManager.UIEvents.OnUpdatePlayerHealthUI -= SetBarValue;
		}
		// Update is called once per frame
		void Update()
		{
			
		}

		#endregion
    
		#region Private Methods

		/// <summary>
		/// Lerps the resource bar to the needed level
		/// </summary>
		/// <param name="setValue"> The level to move the healthbar to </param>
		/// <param name="maxValue"> The max of the bar </param>
		/// <returns></returns>
		private IEnumerator SetBarCoroutine(float setValue, float maxValue) 
		{
			float targetFill = setValue / maxValue;
			
			while (Mathf.Abs(_healthSourceImage.fillAmount - targetFill) > 0.01f) 
			{
				_healthSourceImage.fillAmount = Mathf.Lerp(_healthSourceImage.fillAmount, targetFill, Time.deltaTime * _fillRate);
				yield return null; 
			}
    
			_healthSourceImage.fillAmount = targetFill; // Snap to final value
		}
		
		
		#endregion
    
		#region Public API
    
		/// <summary>
		/// Set the resource bar level
		/// </summary>
		/// <param name="setValue"> The level to move the healthbar to </param>
		/// <param name="maxValue"> The max of the bar </param>
		public void SetBarValue(float setValue, float maxValue)
		{
			// Stop previous coroutine if running
			if (_currentBarCoroutine != null)
				StopCoroutine(_currentBarCoroutine);
    
			_currentBarCoroutine = StartCoroutine(SetBarCoroutine(setValue, maxValue));
		}
		
		#endregion
    
	}

}
