using UnityEngine;

namespace Minibonk.UI
{
	public class HUDManager : UISystem
	{
		#region Variables
		[SerializeField] private ResourceBarUI _healthBarUI;
		[SerializeField] private ResourceBarUI _staminaBarUI;
		#endregion
		
		private void OnEnable()
		{
			_uiManager.UIEvents.OnUpdatePlayerHealthUI += SetHealth;
			_uiManager.UIEvents.OnUpdatePlayerStaminaUI += SetStamina;

		}
		private void OnDisable()
		{
			_uiManager.UIEvents.OnUpdatePlayerHealthUI -= SetHealth;
			_uiManager.UIEvents.OnUpdatePlayerStaminaUI -= SetStamina;
		}
		
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}
		
		#region UI Methods

		private void SetHealth(float value,  float max)
		{
			Debug.Log("SetHealth");
			_healthBarUI.SetBarValue(value, max);
		}

		private void SetStamina(float value, float max)
		{
			_staminaBarUI.SetBarValue(value, max);
		}
		
		#endregion
	}
}

