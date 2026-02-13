using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.UI
{
	public enum UISceneNames
	{
		GameplayHud,
	}
	public class UIManager : MonoBehaviour
	{
		

		#region Variables
		
		public Events UIEvents;

		public UISceneNames gamePlayUIScene;
		
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
			OpenDefaultScenes();
		}

		// Update is called once per frame
		void Update()
		{

		}

		#region Private Methods

		private void OpenDefaultScenes()
		{
			InstantiateUIScene(gamePlayUIScene);
		}

		#endregion
		
		#region Public API
		
		public void InstantiateUIScene(UISceneNames uiSceneNames)
		{
			SceneManager.LoadSceneAsync(uiSceneNames.ToString(),  LoadSceneMode.Additive);
		}

		public void CloseUIScene(UISceneNames uiSceneNames)
		{
			SceneManager.UnloadSceneAsync(uiSceneNames.ToString());
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
		public Action<UISceneNames> OnInstantiateUIScene;
		/// <summary>
		/// Closes a UI scene by its given name
		/// </summary>
		public Action<UISceneNames> OnCloseUIScene;
	}
}
