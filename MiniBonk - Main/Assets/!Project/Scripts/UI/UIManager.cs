using System;
using Minibonk.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.UI
{
	/// <summary>
	/// Name of all available scenes to switch too
	/// </summary>
	public enum UISceneNames
	{
		MainMenu,
		GameplayHud,
	}
	public class UIManager : MonoBehaviour
	{
		

		#region Variables
		
		public Events UIEvents;

		public UISceneNames gamePlayUIScene = UISceneNames.GameplayHud;
		
		#endregion

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += OnSceneChange;
			UIEvents.OnInstantiateUIScene += InstantiateUIScene;
			UIEvents.OnCloseUIScene += CloseUIScene;
			UIEvents.SwapActiveUIScene += SwapUIScene;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnSceneChange;
			UIEvents.OnInstantiateUIScene -= InstantiateUIScene;
			UIEvents.OnCloseUIScene -= CloseUIScene;
			UIEvents.SwapActiveUIScene -= SwapUIScene;
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
			Debug.Log($"Scene {from.name} changed to {to.name}");
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
			if (!SceneManager.GetSceneByName(uiSceneNames.ToString()).isLoaded)
			{
				Debug.Log($"Scene {uiSceneNames.ToString()} not loaded");
				return;
			}
			SceneManager.UnloadSceneAsync(uiSceneNames.ToString());
		}
		
		/// <summary>
		/// Swaps active UI scenes like the Main Menu
		/// </summary>
		/// <param name="uiSceneNames"> the name of the scene to open, selectable via <see cref="UISceneNames"/> </param>
		public void SwapUIScene(UISceneNames uiSceneNames)
		{
			SceneManager.LoadSceneAsync(uiSceneNames.ToString());
		}
		
		#endregion
	}

	public struct Events
	{
		/// <summary>
		/// Updates the players health UI
		/// </summary>
		public delegate void UpdatePlayerHealthUI(float health, float healthMax);
		public UpdatePlayerHealthUI OnUpdatePlayerHealthUI;
		
		/// <summary>
		/// Update the players stamina UI
		/// </summary>
		public delegate void UpdatePlayerStaminaUI(float current, float max);
		public UpdatePlayerStaminaUI OnUpdatePlayerStaminaUI;
		
		/// <summary>
		/// Opens a UI scene by its given name
		/// </summary>
		public Action<UISceneNames> OnInstantiateUIScene;
		/// <summary>
		/// Closes a UI scene by its given name
		/// </summary>
		public Action<UISceneNames> OnCloseUIScene;
		public Action<UISceneNames> SwapActiveUIScene;
	}
}
