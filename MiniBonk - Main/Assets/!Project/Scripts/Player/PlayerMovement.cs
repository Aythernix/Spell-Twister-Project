using System;
using UnityEngine;
using Minibonk.Management;
using Unity.Mathematics;

namespace Minibonk.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables

        // Serialized Fields
        
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 10f;

        [Header("References")] 
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Rigidbody _rb;
        
        // Private Fields
        
        // Inputs
        private InputManager _inputManager;
        
        private Vector2 _directionInput;
        
        // Public Fields
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            _inputManager = new InputManager();
        }

        private void OnEnable()
        {
            _inputManager.Player.Enable();

            // Subscribes to the move event
            // Assigns the direction input vector to Direction Input
            _inputManager.Player.Move.performed += context => _directionInput = context.ReadValue<Vector2>();
            _inputManager.Player.Move.canceled += context => _directionInput = Vector2.zero;
        }

        private void OnDisable()
        {
            _inputManager.Player.Disable();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
           PlayerWalk();
        }

        #endregion

        #region Private Methods

        // Basic Player Movement
        private void PlayerWalk()
        {
            // Get camera forward and right, and ignore tilt
            Vector3 forward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            Vector3 moveDirection = (forward * _directionInput.y) + (right * _directionInput.x);
            
            _rb.linearVelocity = new Vector3(moveDirection.x * _moveSpeed, _rb.linearVelocity.y, moveDirection.z * _moveSpeed);
            
            // Rotate the player to match the camera
            // ONLY if the player is moving
            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); 
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.fixedDeltaTime);
            }
        }
        #endregion
    }
}
