using System;
using MiniBonk.Enemy;
using UnityEngine;

namespace Minibonk.Management
{

	public class GameManager : MonoBehaviour
	{
		#region Variables

		// Game Events
		public readonly EventHandler EventHandler = new EventHandler();

		// Singleton
		public static GameManager Instance;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else if (Instance != this)
			{
				Destroy(this);
			}

			DontDestroyOnLoad(this);

		}
		

		#endregion
	}

	// Game Management Events

	public class EventHandler
	{
		#region Death Events

		/// <summary>
		/// Called when the player dies
		/// </summary>
		public Action OnPlayerDeath;

		/// <summary>
		/// Named delegate so you can see 'killer' and 'victim' in autocomplete
		/// </summary>
		public delegate void EnemyKilledHandler(GameObject killer, Enemy victim);

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