using Unity.Cinemachine;
using UnityEngine;

namespace Minibonk.Player
{
    public class CameraController : MonoBehaviour
    {
        #region Variables

        // Serialized Fields
        [Header("References")] 
        [SerializeField] private CinemachineCamera _thirdPersonCamera;
        [SerializeField] private Rigidbody _rb;
        
        [Header("FOV Settings")]
        [SerializeField] private float _minFov = 60f;
        [SerializeField] private float _maxFov = 80f;
        [SerializeField] private float _maxVelocityThreshold;
        [SerializeField] private float _smoothSpeed = 5;
        
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
            HandleSpeedFOV();
        }
        #endregion

        #region Private Methods

        private void HandleSpeedFOV()
        {
            Vector3 horizontalVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);

            float currentSpeed = horizontalVelocity.magnitude;

            float targetFov = Mathf.Lerp(_minFov, _maxFov, currentSpeed / _maxVelocityThreshold);

            float currentFov = _thirdPersonCamera.Lens.FieldOfView;
            _thirdPersonCamera.Lens.FieldOfView = Mathf.Lerp(currentFov, targetFov, Time.deltaTime * _smoothSpeed);
        }

        #endregion
    }
}

