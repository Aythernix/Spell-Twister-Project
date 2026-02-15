using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.Management
{
	/// <summary>
	/// Gameplay Scenes
	/// </summary>
	public enum SceneNames
	{
		MainLevel,
	}
	public class SceneController : MonoBehaviour
	{
		
			
		
		#region Unity Methods
		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}
		#endregion
		
		#region Public API
		
		/// <summary>
		/// Changes the gameplay scene
		/// </summary>
		/// <param name="sceneName"> Scene to change to </param>
		public void ChangeScene(SceneNames sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName.ToString());
		}
		
		#endregion
	}
}
