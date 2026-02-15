using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minibonk.Management
{
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

		public void ChangeScene(SceneNames sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName.ToString());
		}
		
		#endregion
	}
}
