using System;
using UnityEngine;
using Minibonk.Management;

namespace Minibonk.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum MovementSates
        {
            Sprinting,
            Dashing,
            Sliding,
            Jumping,
            WallJumping,
            Idle,
        }
        
        #region Variables

        // Serialized Fields
        
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 10f;
        [SerializeField] private float _sprintMultiplier = 1.5f;
        
        [Header("Ground Check Settings")]
        [SerializeField] private float _radius;
        [SerializeField] private float _distance;

        [Header("References")] 
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Rigidbody _rb;
        
        // Private Fields
        
        // Inputs
        private InputManager _inputManager;
        private Vector2 _directionInput;

        // Movement Fields
        

        private Vector3 _cameraForward;
        private Vector3 _cameraRight;

        private Vector3 _moveDirection;
        
        // Public Fields
        
        public MovementSates currentMovementState = MovementSates.Idle;
        
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

            _inputManager.Player.Sprint.performed += context => currentMovementState = MovementSates.Sprinting;
            _inputManager.Player.Sprint.canceled += context => currentMovementState = MovementSates.Idle;
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
            HandlePlayerDirection();
            
            // Executes code according the players current actions
            switch (currentMovementState)
            {
                case MovementSates.Idle:
                    HandleBasicMovement(_walkSpeed);
                    break;
                case MovementSates.Sprinting:
                    HandleSprint();
                    break;
            }
        }

        #endregion

        #region Private Methods

        // Basic Player Movement
        private void HandlePlayerDirection()
        {
            // Get camera forward and right, and ignore tilt
            _cameraForward = _cameraTransform.forward;
            _cameraRight = _cameraTransform.right;
            _cameraForward.y = 0;
            _cameraRight.y = 0;
            _cameraForward.Normalize();
            _cameraRight.Normalize();
            
            // Rotate the player to match the camera
            // ONLY if the player is moving
            if (_moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up); 
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.deltaTime);
            }
        }
        
        private void HandleBasicMovement(float speed)
        {
            _moveDirection = (_cameraForward * _directionInput.y) + (_cameraRight * _directionInput.x);
            _rb.linearVelocity = new Vector3(_moveDirection.x * speed, _rb.linearVelocity.y, _moveDirection.z * speed);
        }

        private void HandleSprint()
        {
            HandleBasicMovement(_walkSpeed * _sprintMultiplier);
        }

        private bool HandleGroundDetection()
        {
            Physics.SphereCast(transform.position, _radius, Vector3.down, out RaycastHit hit, _distance);
            return false;
        }
        
        #endregion
    }
}
