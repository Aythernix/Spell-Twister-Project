using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minibonk.UI
{
	public class HealthbarUI : UISystem
	{

		#region Variable

		[SerializeField] private Image _healthSourceImage;
		
		#endregion

		#region Unity Methods

		private void OnEnable()
		{
			_uiManager.UIEvents.updatePlayerHealthUI += SetHealthBarValue;
		}

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
	
		private void SetHealthBarValue(float health,  float maxHealth)
		{
			_healthSourceImage.fillAmount = health / maxHealth;
		}
		
		#endregion
    
		#region Public API
    
		//
		#endregion
    
	}

}
