using System;
using Minibonk.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.UI
{
	public enum UISceneNames
	{
		MainMenu,
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
			SceneManager.activeSceneChanged += OnSceneChange;
			UIEvents.OnInstantiateUIScene += InstantiateUIScene;
			UIEvents.OnCloseUIScene += CloseUIScene;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnSceneChange;
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
		
		// Called whenever a scene is changed
		private void OnSceneChange(Scene from, Scene to)
		{
			// Opens HUD if scene is a playable scene
			if (GameManager.Instance.sceneIsPlayable)
			{
				OpenPlayableScenes();
			}
			else
			{
				ClosePlayableScenes();
			}
		}
		
		// Initiates the gameplay HUD
		private void OpenPlayableScenes()
		{
			InstantiateUIScene(gamePlayUIScene);
		}
		
		// Closes the gameplay HUD
		private void ClosePlayableScenes()
		{
			CloseUIScene(gamePlayUIScene);
		}

		#endregion
		
		#region Public API
		
		/// <summary>
		///	Overlays UI scenes over the currently open scene 
		/// </summary>
		/// <param name="uiSceneNames"> the name of the scene to open, selectable via <see cref="UISceneNames"/> </param>
		public void InstantiateUIScene(UISceneNames uiSceneNames)
		{
			SceneManager.LoadSceneAsync(uiSceneNames.ToString(),  LoadSceneMode.Additive);
		}

		/// <summary>
		/// Closes UI scene overlays
		/// </summary>
		/// <param name="uiSceneNames"> The name of the scene to open, selectable via <see cref="UISceneNames"/> </param>
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
