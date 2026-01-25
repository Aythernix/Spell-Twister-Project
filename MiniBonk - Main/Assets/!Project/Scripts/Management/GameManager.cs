using System;
using UnityEngine;

namespace Minibonk.Management
{

	public class GameManager : MonoBehaviour
	{
		#region Variables

		// Game Events
		public Events Events = new Events();

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
	public struct Events
	{
		// Death Events
		
		/// <summary>
		/// Called when the player dies
		/// </summary>#
		public Action OnPlayerDeath;
		/// <summary>
		/// Called when an enemy is killed
		/// </summary>
		public Action<GameObject> OnEnemyKilled;
		

		// Leveling Events
		
		/// <summary>
		/// Called to increase the player level
		/// </summary>
		public Action<int> IncreasePlayerLevel;
		/// <summary>
		/// Called when the players level increases
		/// </summary>
		public Action<int> OnPlayerLevelIncrease;

		/// <summary>
		/// Called to add XP to the player
		/// </summary>
		public Action<int> AddXp;
		/// <summary>
		/// Called when the player gains XP
		/// </summary>
		public Action<int> OnXpGained;
	}
}
