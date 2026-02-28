using System;
using MiniBonk.Enemy;
using Minibonk.MovementAbilities;
using Minibonk.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minibonk.Management
{
	public class GameManager : MonoBehaviour
	{
		public enum GameState
		{
			Playing,
			Paused,
			NotStarted,
		}
		
		#region Variables

		// Game Events
		public readonly Events EventHandler = new Events();

		// Singleton
		public static GameManager Instance;
		
		// True if the current scene is a playable scene
		public bool sceneIsPlayable = false;
		
		// True if the game can be paused
		public bool allowPause = true;
		
		// The current active level manager
		public LevelManager currentLevelManager;

		public GameState currentGameState;

		#endregion

		#region SubManagers

		public UIManager uiManager { get; private set; }
		public SceneController sceneController { get; private set; }

		#endregion

		#region Unity Methods

		private void Awake()
		{
			SetSubManagers();
			
			if (Instance == null)
			{
				Instance = this;
			}
			else if (Instance != this)
			{
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}
		
		#endregion
		
		#region Private Methods

		private void SetSubManagers()
		{
			uiManager = GetComponentInChildren<UIManager>();
			sceneController = GetComponentInChildren<SceneController>();
		}

		public void PauseInput(InputAction.CallbackContext context)
		{

			if (context.performed)
			{
				switch (currentGameState)
				{
					case GameState.Playing:
						PauseGame();
						break;
					case GameState.Paused:
						UnPauseGame();
						break;
					default:
						break;
				}
			}
		}

		private void PauseGame()
		{
			if(!allowPause) return;
			if(!sceneIsPlayable) return; 
			uiManager.UIEvents.OnInstantiateUIScene(UISceneNames.PauseMenu, true);
		}

		public void UnPauseGame()
		{
			if(!allowPause) return;
			if(!sceneIsPlayable) return; 
			uiManager.UIEvents.OnCloseUIScene(UISceneNames.PauseMenu, true);
		}
		
		#endregion
	}

	// Game Management Events

	public class Events
	{
		#region Death Events

		/// <summary>
		/// Called when the player dies
		/// </summary>
		public Action OnPlayerDeath;

		/// <summary>
		/// Named delegate so you can see 'killer' and 'victim' in autocomplete
		/// </summary>
		public delegate void EnemyKilledHandler(GameObject killer, GameObject victim);

		public EnemyKilledHandler OnEnemyKilled;

		#endregion

		#region Leveling Events

		/// <summary>
		/// Called to request a player level increase (parameter: amountOfLevels)
		/// </summary>
		public Action<int> IncreasePlayerLevel;

		/// <summary>
		/// Called when the player level successfully changes (parameter: newLevel)
		/// </summary>
		public Action<int> OnPlayerLevelIncrease;

		/// <summary>
		/// Called to request XP be added to the player (parameter: xpAmount)
		/// </summary>
		public Action<int> AddXp;

		/// <summary>
		/// Called when the player actually receives XP (parameter: currentTotalXp)
		/// </summary>
		public Action<int> OnXpGained;

		#endregion
		
		#region Inventory Events

		/// <summary>
		/// Called when an ability is equipped (parameters: slotIndex, ability)
		/// </summary>
		/// <param name="slotIndex"></param>
		/// <param name="ability"></param>
		public Action<int, MovementAbility> OnAbilityEquipped;
		
		/// <summary>
		/// Called when an ability is unequipped (parameters: slotIndex, ability)
		/// </summary>
		/// <param name="slotIndex"></param>
		/// <param name="ability"></param>
		public Action<int, MovementAbility> OnAbilityUnequipped;
		
		/// <summary>
		/// Called when an ability slot is triggered (parameter: slotIndex)
		/// </summary>
		/// <param name="slotIndex"></param>
		public Action<int> OnAbilitySlotTriggered;

		#endregion
	}
}