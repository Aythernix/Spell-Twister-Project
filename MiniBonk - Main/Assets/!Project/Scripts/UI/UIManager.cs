using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.UI
{
	public class UIManager : MonoBehaviour
	{
		#region Variables
		
		public Events UIEvents = new Events();
		
		#endregion

		private void OnEnable()
		{
			UIEvents.OnInstantiateUIScene += InstantiateUIScene;
			UIEvents.OnCloseUIScene += CloseUIScene;
		}

		private void OnDisable()
		{
			UIEvents.OnInstantiateUIScene -= InstantiateUIScene;
			UIEvents.OnCloseUIScene -= CloseUIScene;
		}

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		#region Private Methods

		private void InstantiateUIScene(string sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName,  LoadSceneMode.Additive);
		}

		private void CloseUIScene(string sceneName)
		{
			SceneManager.UnloadSceneAsync(sceneName);
		}

		#endregion
	}

	public struct Events
	{
		/// <summary>
		/// Updates the players health UI
		/// </summary>
		public delegate void UpdatePlayerHealthUI(float health, float healthMax);
		public UpdatePlayerHealthUI updatePlayerHealthUI;
		
		/// <summary>
		/// Opens a UI scene by its given name
		/// </summary>
		public Action<string> OnInstantiateUIScene;
		/// <summary>
		/// Closes a UI scene by its given name
		/// </summary>
		public Action<string> OnCloseUIScene;
	}
}
