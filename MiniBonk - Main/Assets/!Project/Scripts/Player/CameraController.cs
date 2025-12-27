using Unity.Cinemachine;
using UnityEngine;

namespace Minibonk.Player
{
    public class CameraController : MonoBehaviour
    {
        #region Variables
        [Header("References")] 
        [SerializeField] private CinemachineCamera _thirdPersonCamera;
        [SerializeField] private Rigidbody _rb;
        
        [Header("FOV Settings")]
        [SerializeField] private float _minFov = 60f;
        [SerializeField] private float _maxFov = 80f;
        [SerializeField] private float _maxVelocityThreshold = 15f;
        [SerializeField] private float _smoothSpeed = 5f;
        
     

        #endregion

        #region Unity Methods
        
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _thirdPersonCamera.Lens.FieldOfView = _minFov;
        }
        
        void Update()
        {
            HandleSpeedFOV();
        }

        #endregion

        #region Private Methods

        // Adjusts the Camera FOV based on the Rigidbody's current linear velocity
        private void HandleSpeedFOV()
        {
            // Get horizontal velocity only (ignore falling/jumping)
            Vector3 horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            float currentSpeed = horizontalVelocity.magnitude;

            // Calculate target FOV using Lerp
            float t = currentSpeed / Mathf.Max(_maxVelocityThreshold, 0.01f);
            float targetFov = Mathf.Lerp(_minFov, _maxFov, t);

            // Smoothly apply the FOV change
            float currentFov = _thirdPersonCamera.Lens.FieldOfView;
            _thirdPersonCamera.Lens.FieldOfView = Mathf.Lerp(currentFov, targetFov, Time.deltaTime * _smoothSpeed);
        }

        #endregion
    }
}