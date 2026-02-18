using System;
using UnityEngine;
using Minibonk.Management;
using RotaryHeart.Lib.PhysicsExtension;
using UnityEditor.Build;
using UnityEngine.Serialization;

//using Physics = RotaryHeart.Lib.PhysicsExtension.Physics;

namespace Minibonk.Player
{
	public class PlayerMovement : PlayerSystem
	{

		#region Variables

		[Header("Movement Settings")] [SerializeField]
		private float _walkSpeed = 10f;

		[SerializeField] private float _jumpForce = 5f;
		[SerializeField] private float _jumpBuffer = 0.5f;
		[SerializeField] private float _coyoteTime = 0.5f;

		[Header("Ground Check Settings")] [SerializeField]
		private float _radius = 0.5f;

		[SerializeField] private float _distance = 0.2f;
		[SerializeField] private LayerMask _groundLayer;

		[Header("Snapping Settings")] [SerializeField]
		private float _maxSnapAngle;

		[SerializeField] private float _minSnapAngle;
		[SerializeField] private float _snappingForce;

		[Header("References")] [SerializeField]
		private Transform _cameraTransform;

		[SerializeField] private Rigidbody _rb;

		private bool _isJumping = false;
		private bool _resetMovement = true;

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
			_playerHandler.PlayerEvents.OnMove += HandleMoveInput;
			_playerHandler.PlayerEvents.OnJumpStarted += HandleJumpInput;
			_playerHandler.PlayerEvents.PreventMovementInterference += HandlePreventMovementInterference;
		}

		private void OnDisable()
		{
			_playerHandler.PlayerEvents.OnMove -= HandleMoveInput;
			_playerHandler.PlayerEvents.OnJumpStarted -= HandleJumpInput;
		}

		private void FixedUpdate()
		{
			// Update Directions
			UpdateCameraVectors();
			bool isGrounded = HandleGroundDetection();

			HandleJumpCheck(isGrounded);

			ApplyVelocity(_walkSpeed * speedMultiplier);

			SnapToGround();

			HandleRotation();
		}

		#endregion

		#region Private Methods
		
		// Normalizes input
		private void HandleMoveInput(Vector2 input) => _directionInput = input.normalized;
		private void HandleJumpInput() => _jumpBufferCounter = _jumpBuffer;

		// Stops movement controls
		private void HandlePreventMovementInterference(bool state) => _resetMovement = !state;
		
		private void UpdateCameraVectors()
		{
			_cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
			_cameraRight = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;
		}
		
		// Rotates the player to match the camera
		private void HandleRotation()
		{
			// Only rotates when moving
			if (_moveDirection.sqrMagnitude > 0.01f)
			{
				Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
				transform.rotation =
					Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.fixedDeltaTime);
			}
		}

		// Applies velocity to the player
		private void ApplyVelocity(float speed)
		{
			_moveDirection = (_cameraForward * _directionInput.y) + (_cameraRight * _directionInput.x).normalized;
			if (_resetMovement)
			{
				_rb.linearVelocity =
					new Vector3(_moveDirection.x * speed, _rb.linearVelocity.y, _moveDirection.z * speed);
			}
		}
		
		// Snaps to the ground when not jumping
		private void SnapToGround()
		{
			UnityEngine.Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f, _groundLayer);
			if (hit.collider != null)
			{
				if (_isJumping) return;

				var normal = hit.normal;
				var angle = Vector3.Angle(normal, Vector3.down);
				// Debug.Log(angle);

				if ((_minSnapAngle < angle && angle < _maxSnapAngle) && !HandleGroundDetection())
				{
					_rb.linearVelocity = -hit.normal * _snappingForce;
				}
			}
		}

		// Detects ground via a sphere cast
		private bool HandleGroundDetection()
		{
			// #if UNITY_EDITOR 
			// // Uses the custom library to draw debug lines in the Scene view
			// bool grounded = RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(
			// 	transform.position, _radius, Vector3.down, out RaycastHit hit,
			// 	_distance, _groundLayer, PreviewCondition.Both, 0.1f, Color.green, Color.red);
			// #else
			bool grounded = UnityEngine.Physics.SphereCast(
				transform.position, _radius, Vector3.down, out RaycastHit hit, 
				 _distance, _groundLayer);
			// #endif

			// If we are touching the ground and moving downwards/stationary, we are no longer "Jumping"
			if (grounded && _rb.linearVelocity.y <= 0.01f)
			{
				_isJumping = false;
			}

			return grounded;
		}
		
		// Applies buffer and coyote time
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
            _isJumping = true;
            
            // Reset Y velocity before jump for consistent height
            //_rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);


            // Consume Counters preventing double jump
            _jumpBufferCounter = 0;
            _coyoteTimeCounter = 0;
        }
        #endregion
    }
}