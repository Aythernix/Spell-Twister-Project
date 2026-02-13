using UnityEngine;

namespace Minibonk.UI
{
	public class HUDManager : UISystem
	{
		#region Variables
		[SerializeField] private ResourceBarUI _healthBarUI;
		#endregion
		
		private void OnEnable()
		{
			_uiManager.UIEvents.updatePlayerHealthUI += SetHealth;

		}
		private void OnDisable()
		{
			_uiManager.UIEvents.updatePlayerHealthUI -= SetHealth;
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
		
		#endregion
	}
}

