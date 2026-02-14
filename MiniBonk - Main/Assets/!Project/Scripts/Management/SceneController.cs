using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.Management
{
	public class SceneController : MonoBehaviour
	{
		public enum SceneNames
		{
			
		}
			
		
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

		public void ChangeScene(string sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName);
		}
		
		#endregion
	}
}
