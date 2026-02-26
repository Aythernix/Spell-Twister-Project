using UnityEngine;
using Minibonk.Management;

namespace Minibonk.UI
{
	public class MainMenuManager : MonoBehaviour
	{
		#region Variables
		private SceneController _sceneController;
		#endregion
		
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
			_sceneController = GameManager.Instance.sceneController;
			GameManager.Instance.currentGameState = GameManager.GameState.NotStarted;
		}

		// Update is called once per frame
		void Update()
		{

		}

		#region Button Events
		
		// Called when the player clicks start
		public void OnStartClick()
		{
			_sceneController.ChangeScene(SceneNames.MainLevel);
		}

		public void OnSettingsClicked()
		{
			
		}

		public void OnQuitClicked()
		{
			
		}

		#endregion
	}
}
