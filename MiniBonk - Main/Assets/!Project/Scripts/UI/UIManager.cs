using UnityEngine;

namespace Minibonk.UI
{
	public class UIManager : MonoBehaviour
	{
		#region Variables
		
		public Events UIEvents = new Events();
		#endregion

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}

	public struct Events
	{
		/// <summary>
		/// Updates the players health UI
		/// </summary>
		public delegate void UpdatePlayerHealthUI(float health, float healthMax);
		public UpdatePlayerHealthUI updatePlayerHealthUI;
	}
}
