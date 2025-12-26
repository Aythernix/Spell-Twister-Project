using Unity.Cinemachine;
using UnityEngine;

namespace Minibonk.Player
{
    public class CameraController : MonoBehaviour
    {
        #region Variables

        // Serialized Fields
        [Header("Refrences")] 
        [SerializeField] private CinemachineCamera _thirdPersonCamera;
        // Private Fields
        
        // Public Fields

        #endregion

        #region Unity Methods
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        #endregion
    }
}

