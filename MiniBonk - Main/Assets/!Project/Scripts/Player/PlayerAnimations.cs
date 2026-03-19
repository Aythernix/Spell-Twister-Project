using System;
using Minibonk.Management;
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
        
        [SerializeField] private ParticleSystem grassFootsteps; 

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
                 return;
            }
             
            isJumping = true;
            _animator.SetTrigger("Jumping");
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

            if (!isDashing && !isJumping)
            {
                isSprinting = true;
                _animator.SetBool("Sprinting", true);
            }
        }
        

        #endregion

        public void PlayFootSteps()
        {
            if (_playerHandler.GetComponent<PlayerMovement>().isGrounded)
            {
                GameManager.Instance.EventHandler.PlayAudio?.Invoke(AudioManager.AudioClips.PlayerFootsteps, _playerHandler.playerAudioSource);
                if (grassFootsteps != null)
                {
                    grassFootsteps.Play();
                }
            }
            
        }
        
        public void PlaySprint()
        {
            if (_playerHandler.GetComponent<PlayerMovement>().isGrounded)
            {
                GameManager.Instance.EventHandler.PlayAudio?.Invoke(AudioManager.AudioClips.PlayerSprint, _playerHandler.playerAudioSource);
                if (grassFootsteps != null)
                {
                    var main = grassFootsteps.main; // Access the main module of the particle system
                    main.simulationSpeed = 2f; // Set the simulation speed to 2x for sprinting
                    grassFootsteps.Play();
                }
            }
        }   
    }
}
