using Minibonk.Management;
using Minibonk.Player;
using TMPro;
using UnityEngine;

namespace Minibonk.UI
{
	public class HUDManager : UISystem
	{
		#region Variables
		[SerializeField] private ResourceBarUI _healthBarUI;
		[SerializeField] private ResourceBarUI _staminaBarUI;
		[SerializeField] private ResourceBarUI _xpBarUI;
		[SerializeField] private TextMeshProUGUI _levelText;
		#endregion
		
		private void OnEnable()
		{
			_uiManager.UIEvents.OnUpdatePlayerHealthUI += SetHealth;
			_uiManager.UIEvents.OnUpdatePlayerStaminaUI += SetStamina;
			_uiManager.UIEvents.OnUpdatePlayerXpUi += SetXP;
			_uiManager.UIEvents.OnUpdateLevelTextUI += SetLevel;

		}
		private void OnDisable()
		{
			_uiManager.UIEvents.OnUpdatePlayerHealthUI -= SetHealth;
			_uiManager.UIEvents.OnUpdatePlayerStaminaUI -= SetStamina;
			_uiManager.UIEvents.OnUpdatePlayerXpUi -= SetXP;
			_uiManager.UIEvents.OnUpdateLevelTextUI -= SetLevel;
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
		
		private void SetXP(float value, float max)
		{
			_xpBarUI.SetBarValue(value, max);
		}

		private void SetLevel(int level)
		{
			_levelText.text = level.ToString();
		}
		
		#endregion
	}
}

