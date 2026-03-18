using System;
using System.Collections.Generic;
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
		PauseMenu,
		GameplayHud,
		LevelUpPopup,
		Shop,
		Spells,
		Settings,
		Credits,
	}
	public class UIManager : MonoBehaviour
	{
		

		#region Variables
		
		public Events UIEvents = new Events();

		public UISceneNames gamePlayUIScene = UISceneNames.GameplayHud;
		
		private Stack<int> _closeableSceneStack = new Stack<int>();
		
		#endregion

		private void OnEnable()
		{
			Application.backgroundLoadingPriority = ThreadPriority.Low;
			SceneManager.activeSceneChanged += OnSceneChange;
			UIEvents.OnInstantiateUIScene += InstantiateUIScene;
			UIEvents.OnInstantiatePersistantUIScene += OpenPersistantUI;
			UIEvents.OnCloseUIScene += CloseUIScene;
			UIEvents.SwapActiveUIScene += SwapUIScene;
			UIEvents.UpdateUI += UpdateUI;
			UIEvents.ClearCloseableSceneStack += ClearCloseableSceneStack;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnSceneChange;
			UIEvents.OnInstantiateUIScene -= InstantiateUIScene;
			UIEvents.OnInstantiatePersistantUIScene -= OpenPersistantUI;
			UIEvents.OnCloseUIScene -= CloseUIScene;
			UIEvents.SwapActiveUIScene -= SwapUIScene;
			UIEvents.UpdateUI -= UpdateUI;
			UIEvents.ClearCloseableSceneStack -= ClearCloseableSceneStack;
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

		private void UpdateUI()
		{
			UIEvents.OnUpdatePlayerHealthUI?.Invoke(GameManager.Instance.currentLevelManager.playerHandler.playerData.currentHealth, GameManager.Instance.currentLevelManager.playerHandler.playerData.variableMaxHealth);
			UIEvents.OnUpdatePlayerStaminaUI?.Invoke(GameManager.Instance.currentLevelManager.playerHandler.playerData.currentStamina, GameManager.Instance.currentLevelManager.playerHandler.playerData.variableMaxStamina);
			UIEvents.OnUpdateLevelTextUI?.Invoke(GameManager.Instance.currentLevelManager.playerHandler.playerData.level);
			UIEvents.OnUpdatePlayerMoneyUI?.Invoke(GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetMoney.Invoke());
			UIEvents.OnUpdatePlayerTracePoints?.Invoke(GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetTracePoint.Invoke());
		}
		// Called whenever a scene is changed
		private void OnSceneChange(Scene from, Scene to)
		{
			Debug.Log($"Scene {from.name} changed to {to.name}");
			// Opens HUD if scene is a playable scene
			if (GameManager.Instance.sceneIsPlayable)
			{
				OpenGamePlayUI();
			}
			else
			{
				ClosePlayableScenes();
			}
		}
		
		// Initiates the gameplay HUD
		private void OpenGamePlayUI()
		{
			InstantiateUIScene(gamePlayUIScene, false);
		}
		
		// Closes the gameplay HUD
		private void ClosePlayableScenes()
		{
			CloseUIScene(gamePlayUIScene, true);
		}

		#endregion
		
		#region Public API

		///  <summary>
		/// 	Overlays UI scenes over the currently open scene 
		///  </summary>
		///  <param name="uiSceneNames"> the name of the scene to open, selectable via <see cref="UISceneNames"/> </param>
		///  <param name="pause"> Should the game pause true by default</param>
		public void InstantiateUIScene(UISceneNames uiSceneNames, bool pause = true)
		{
			SceneManager.LoadSceneAsync(uiSceneNames.ToString(),  LoadSceneMode.Additive);
			if (pause)
			{
				GameManager.Instance.PauseGame();
			}

			if (GameManager.Instance.currentGameState == GameManager.GameState.Paused)
			{
				_closeableSceneStack.Push(SceneManager.GetSceneByName(uiSceneNames.ToString()).buildIndex);
			}
		}

		public void OpenPersistantUI(UISceneNames uiSceneNames, bool pause = true)
		{
			SceneManager.LoadSceneAsync(uiSceneNames.ToString(),  LoadSceneMode.Additive);
			if (pause)
			{
				GameManager.Instance.PauseGame();
			}
		}

		public void CloseClosableSceneStack()
		{
			bool stackWasUsed = false;
			if (_closeableSceneStack.Count > 0)
			{
				SceneManager.UnloadSceneAsync(_closeableSceneStack.Pop());
				stackWasUsed = true;
			}
			
			if (_closeableSceneStack.Count == 0 && stackWasUsed)
			{
				GameManager.Instance.UnPauseGame();
			}
		}
		
		public void ClearCloseableSceneStack()
		{
			_closeableSceneStack.Clear();
		}
		
        

		/// <summary>
		/// Closes UI scene overlays
		/// </summary>
		/// <param name="uiSceneNames"> The name of the scene to open, selectable via <see cref="UISceneNames"/> </param>
		/// <param name="unpause"> Should the game Unpause true by default</param>
		public void CloseUIScene(UISceneNames uiSceneNames, bool unpause = true)
		{
			if (!SceneManager.GetSceneByName(uiSceneNames.ToString()).isLoaded)
			{
				Debug.Log($"Scene {uiSceneNames.ToString()} not loaded");
				return;
			}
			SceneManager.UnloadSceneAsync(uiSceneNames.ToString());

			if (unpause)
			{
				GameManager.Instance.UnPauseGame();
			}

			if (_closeableSceneStack.Contains(SceneManager.GetSceneByName(uiSceneNames.ToString()).buildIndex))
			{
				_closeableSceneStack.Pop();
			}
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
		
		public delegate void UpdatePlayerXpUi(float current, float max);
		public UpdatePlayerXpUi OnUpdatePlayerXpUi;
		
		public delegate void UpdatePlayerTracePoints(int tracePoints);
		public UpdatePlayerTracePoints OnUpdatePlayerTracePoints;
		
		public delegate void UpdatePlayerMoneyUI(int money);
		public UpdatePlayerMoneyUI OnUpdatePlayerMoneyUI;

		public Action<int> OnUpdateLevelTextUI;
		
		/// <summary>
		/// Opens a UI scene by its given name
		///
		/// Takes the scene name, and if the game should pause 
		/// </summary>
		public Action<UISceneNames, bool> OnInstantiateUIScene;

		public Action<UISceneNames, bool> OnInstantiatePersistantUIScene;
		/// <summary>
		/// Closes a UI scene by its given name
		///
		/// Takes the scene name, and if the game should unpause
		/// </summary>
		public Action<UISceneNames, bool> OnCloseUIScene;
		public Action<UISceneNames> SwapActiveUIScene;

		public Action UpdateUI;

		public Action ClearCloseableSceneStack;
	}
}
