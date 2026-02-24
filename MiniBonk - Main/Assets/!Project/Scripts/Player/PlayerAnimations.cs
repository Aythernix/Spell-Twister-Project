using System;
using Minibonk.Player;
using UnityEditor.Animations;
using UnityEngine;

namespace Minibonk.Player
{
    public class PlayerAnimations : PlayerSystem
    {
        private static readonly int Moving = Animator.StringToHash("Moving");
        #region Variables

        private Rigidbody _rb;
        private Animator _animator;

        //Animation Variables
        public bool isMoving;
        public bool isJumping;
        public bool isDashing;
        public bool isSprinting;


        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        #region Unity Methods

        private void OnEnable()
        {
            _playerHandler.PlayerEvents.OnJumpSucceeded += CheckForJumping;
            _playerHandler.PlayerEvents.OnJumpLanded += CheckForJumping;
            _playerHandler.PlayerEvents.OnDashStarted += CheckForDashing;
            _playerHandler.PlayerEvents.OnDashFinished += CheckForDashing;
            _playerHandler.PlayerEvents.OnSprintStarted += CheckForSprinting;
            _playerHandler.PlayerEvents.OnSprintFinished += CheckForSprinting;
        }

        private void OnDisable()
        {
            _playerHandler.PlayerEvents.OnJumpSucceeded -= CheckForJumping;
            _playerHandler.PlayerEvents.OnJumpLanded -= CheckForJumping;
            _playerHandler.PlayerEvents.OnDashStarted -= CheckForDashing;
            _playerHandler.PlayerEvents.OnDashFinished -= CheckForDashing;
            _playerHandler.PlayerEvents.OnSprintStarted -= CheckForSprinting;
            _playerHandler.PlayerEvents.OnSprintFinished -= CheckForSprinting;
        }

        void Start()
        {
            _rb = transform.root.GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckForMovementAnim();
        }

        #endregion

        #region Private Methods

        // Animation Checks
        private void CheckForMovementAnim()
        {
            if (_rb.linearVelocity.magnitude > 0.1f && !isJumping && !isSprinting && !isDashing)
            {
                isMoving = true;
                _animator.SetBool("Moving", true);
            }
            else
            {
                isMoving = false;
                _animator.SetBool("Moving", false);
            }
        }

        private void CheckForJumping()
        {
            if (isJumping)
            {
                isJumping = false;
                _animator.SetBool("Jumping", false);
                return;
            }

            isJumping = true;
            _animator.SetBool("Jumping", true);
        }

        private void CheckForDashing()
        {
            if (isDashing)
            {
                isDashing = false;
                _animator.SetBool("Dashing", false);
                return;
            }

            isDashing = true;
            _animator.SetBool("Dashing", true);
        }

        private void CheckForSprinting()
        {
            if (isSprinting)
            {
                isSprinting = false;
                _animator.SetBool("Sprinting", false);
                return;
            }

            isSprinting = true;
            _animator.SetBool("Sprinting", true);
        }

        #endregion
    }
}
