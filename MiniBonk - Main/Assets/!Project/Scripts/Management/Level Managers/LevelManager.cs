using System;
using Minibonk.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.Management
{
	public class LevelManager : MonoBehaviour
	{
		
		public enum SceneTypes
		{
			Playable,
			UI
		}
		
		#region Variables
		
		[Header("Level Manager")]
		[field: SerializeField] public SceneTypes sceneType {get; private set;}
		[field: SerializeField] public string sceneName {get; protected set;}
		
		[field: SerializeField] public PlayerHandler playerHandler {get; private set;}
		
		#endregion

		#region Unity Methods

		private void Awake()
		{
			sceneName = SceneManager.GetActiveScene().name;
			SetGameManagerAsPlayable();
			SetLevelManagerInGameManager();
			SetPlayer();
		}

		private void Start()
		{
			InitializeScene();
		}

		#endregion
		
		#region Private Methods

		// Toggles the playable flag
		private void SetGameManagerAsPlayable()
		{
			if (sceneType == SceneTypes.Playable)
			{
				GameManager.Instance.sceneIsPlayable = true;
			}
			else
			{
				GameManager.Instance.sceneIsPlayable = false;
			}
		}
		
		// Sets this as the active level manager
		private void SetLevelManagerInGameManager()
		{
			GameManager.Instance.currentLevelManager = this;
		}

		private void SetPlayer()
		{
			playerHandler = FindFirstObjectByType<PlayerHandler>();
		}
		
		#endregion
		
		#region Protected Methods
		
		/// <summary>
		/// Runs whenever a new scene starts
		/// </summary>
		protected virtual void InitializeScene()
		{
			
		}
		
		#endregion
	}
}

