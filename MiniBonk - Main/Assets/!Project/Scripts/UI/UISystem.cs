using Minibonk.Management;
using UnityEngine;

namespace Minibonk.UI
{
	public abstract class UISystem : MonoBehaviour
	{

		#region Variables

		protected UIManager _uiManager;

		#endregion

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		protected virtual void Awake()
		{
			_uiManager = GameManager.Instance.GetComponentInChildren<UIManager>();
		}

		// Update is called once per frame
		void Update()
		{

		}

		#region Private Method
		//

		#endregion
	}

}