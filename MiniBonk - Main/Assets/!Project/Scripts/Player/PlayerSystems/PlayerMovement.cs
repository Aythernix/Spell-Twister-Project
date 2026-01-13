using System;
using UnityEngine;
using Minibonk.Management;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEngine.Serialization;

namespace Minibonk.Player
{
    public class PlayerMovement : PlayerSystem
    {
        // TODO Add Proper Comments
        public enum MovementSates { Sprinting, Dashing, Sliding, Jumping, Idle }
        
        #region Variables
        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = 10f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _jumpBuffer = 0.5f;
        [SerializeField] private float _coyoteTime = 0.5f;
        
        [Header("Ground Check Settings")]
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _distance = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("References")] 
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Rigidbody _rb;
        
        private InputManager _inputManager;
        private Vector2 _directionInput;

        private float _jumpBufferCounter;
        private float _coyoteTimeCounter;
        

        private Vector3 _cameraForward;
        private Vector3 _cameraRight;
        private Vector3 _moveDirection;
        
        public float speedMultiplier { get; set; } = 1f;
        
        
        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
        } 

        private void OnEnable()
        {
            _playerHandler.events.OnMove += HandleMoveInput;
            _playerHandler.events.OnJumpStarted += HandleJumpInput;
            
        }
        
        private void OnDisable()
        {
            _playerHandler.events.OnMove -= HandleMoveInput;
            _playerHandler.events.OnJumpStarted -= HandleJumpInput;
        }

        private void FixedUpdate()
        {
            // Update Directions
            UpdateCameraVectors();
            bool isGrounded = HandleGroundDetection();
            
            HandleJumpCheck(isGrounded);
            
            ApplyVelocity(_walkSpeed * speedMultiplier);
            
            HandleRotation();
        }
        #endregion

        #region Private Methods
        private void HandleMoveInput(Vector2 input) => _directionInput = input;
        private void HandleJumpInput() => _jumpBufferCounter = _jumpBuffer;
        private void UpdateCameraVectors()
        {
            _cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
            _cameraRight = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;
        }

        private void HandleRotation()
        {
            if (_moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up); 
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.fixedDeltaTime);
            }
        }
        
        private void ApplyVelocity(float speed)
        {
            _moveDirection = (_cameraForward * _directionInput.y) + (_cameraRight * _directionInput.x);
            _rb.linearVelocity = new Vector3(_moveDirection.x * speed, _rb.linearVelocity.y, _moveDirection.z * speed);
        }

        private bool HandleGroundDetection()
        {
            return RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(transform.position, _radius, Vector3.down, out RaycastHit hit, _distance, _groundLayer, PreviewCondition.Both, 0.1f, Color.green, Color.red);
        }

        private void HandleJumpCheck(bool groundCheck)
        {
            
            _jumpBufferCounter -= Time.fixedDeltaTime;
                

            if (!groundCheck)
            {
                _coyoteTimeCounter -= Time.fixedDeltaTime;
            }
            else
            {
                _coyoteTimeCounter = _coyoteTime;
            }

            if (_coyoteTimeCounter > 0 && _jumpBufferCounter > 0)
            {
                PerformJump();
            }
        }

        private void PerformJump()
        {
            // Reset Y velocity before jump for consistent height
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            // Consume Counters preventing double jump
            _jumpBufferCounter = 0;
            _coyoteTimeCounter = 0;
        }
        #endregion
    }
}