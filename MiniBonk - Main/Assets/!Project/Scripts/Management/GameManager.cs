using System;
using MiniBonk.Enemy;
using Minibonk.MovementAbilities;
using Minibonk.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
		
		// The current game settings
		public GameSettings gameSettings;

		public GameState currentGameState;

		public float growthRate = 1.2f;

		public int killCount;

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

			EventHandler.OnEnemyKilled += IncreaseKillCount;
		}
		
		#endregion
		
		#region Private Methods

		private void SetSubManagers()
		{
			uiManager = GetComponentInChildren<UIManager>();
			sceneController = GetComponentInChildren<SceneController>();
		}
		
		#endregion

		public void PauseInput(InputAction.CallbackContext context)
		{

			if (context.performed)
			{
				switch (currentGameState)
				{
					case GameState.Playing:
						OpenPauseMenu();
						break;
					case GameState.Paused:
						ClosePauseMenu();
						break;
					default:
						break;
				}
			}
		}

		public void OpenPauseMenu()
		{
			if(!allowPause) return;
			if(!sceneIsPlayable) return; 
			uiManager.UIEvents.OnInstantiateUIScene(UISceneNames.PauseMenu, true);
		}

		public void ClosePauseMenu()
		{
			if(!allowPause) return;
			if(!sceneIsPlayable) return; 
			// uiManager.UIEvents.OnCloseUIScene(UISceneNames.PauseMenu, true);
			uiManager.CloseClosableSceneStack();
		}

		public void PauseGame()
		{
			Time.timeScale = 0;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			currentGameState = GameState.Paused;
		}

		public void UnPauseGame()
		{
			Time.timeScale = 1;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Instance.currentGameState = GameState.Playing;
		}

		public bool Probability(float chance, float maxChance = 100)
		{
			var randomValue = Random.Range(0f, maxChance);

			if (randomValue <= chance)
			{
				return true;
			}
			return false;
		}

		public float ChaosMultiplier(float _growthRate, int lvlToSubtract = 1)
		{
			if (currentLevelManager?.playerHandler?.playerData == null)
			{
				Debug.LogWarning("ChaosMultiplier: Missing references, returning base 1f.");
				return 1f; 
			}

			int currentLevel = currentLevelManager.playerHandler.playerData.level;

			if (currentLevel <= 1)
				return 1f;

			float chaosMultiplier = 1f + (_growthRate * (currentLevel - lvlToSubtract));
			return chaosMultiplier;
		}
		public float ChaosMultiplier()
		{
			if (currentLevelManager?.playerHandler?.playerData == null)
			{
				Debug.LogWarning("ChaosMultiplier: Missing references, returning base 1f.");
				return 1f; 
			}

			int currentLevel = currentLevelManager.playerHandler.playerData.level;

			if (currentLevel <= 1)
				return 1f;

			float chaosMultiplier = 1f + (growthRate * (currentLevel - 1));
			return chaosMultiplier;
		}

		private void IncreaseKillCount(GameObject killer, GameObject victim)
		{
			Debug.Log(killer);
			if(killer != currentLevelManager.playerHandler.gameObject) return;
			killCount++;
		}


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
		
		/// <summary>
		/// Plays audio from the audio Manager, Takes the audio clip and Source
		/// </summary>
		public Action<AudioManager.AudioClips, AudioSource> PlayAudio;
		
		/// <summary>
		/// Plays a custom audio in the Audio Manager, takes the audio clip,volume and source.
		/// </summary>
		public Action<AudioClip, float, AudioSource> PlayCustomAudio;
		
		/// <summary>
		/// Plays a looping audio clip in the Audio Manager, takes the audio clip and source.
		/// </summary>
		public Action<AudioClip, AudioSource> StartLoopingAudio;
		
		/// <summary>
		/// Stops a looping audio clip in the Audio Manager, takes the source.
		/// </summary>
		public Action<AudioSource> StopLoopingAudio;

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