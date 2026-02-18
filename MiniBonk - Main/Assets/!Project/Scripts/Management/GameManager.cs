using System;
using MiniBonk.Enemy;
using Minibonk.UI;
using UnityEngine;

namespace Minibonk.Management
{
	public class GameManager : MonoBehaviour
	{
		#region Variables

		// Game Events
		public readonly Events EventHandler = new Events();

		// Singleton
		public static GameManager Instance;
		
		// True if the current scene is a playable scene
		public bool sceneIsPlayable = false;
		
		// The current active level manager
		public LevelManager currentLevelManager;

		#endregion

		#region SubManagers

		public UIManager uiManager { get; private set; }
		public EnemyManager enemyManager { get; private set; }
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
			enemyManager = GetComponent<EnemyManager>();
			sceneController = GetComponentInChildren<SceneController>();
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
	}
}