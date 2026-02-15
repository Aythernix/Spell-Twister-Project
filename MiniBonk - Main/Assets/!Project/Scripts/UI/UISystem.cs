using Minibonk.Management;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minibonk.UI
{
	public abstract class UISystem : MonoBehaviour
	{

		#region Variables
		
		private bool isHovered = false;
		private bool isSelected = false;

		protected UIManager _uiManager;
		
		

		#endregion

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		protected virtual void Awake()
		{
			_uiManager = GameManager.Instance.uiManager;
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		#region Private Method
		//

		#endregion
		
		#region Protected Methods

		//
		
		#endregion
	}

}