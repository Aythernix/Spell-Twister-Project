using System;
using Minibonk.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minibonk.Management
{
    // TODO Add Comments and summaries
    public class InputHandler : PlayerSystem
    {
        #region Variables
        private InputManager _inputManager;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();
            _inputManager = new InputManager();
        }

        private void OnEnable()
        {
            _inputManager.Enable();
            
            // Bind Input Actions to Named Methods
            _inputManager.Player.Move.performed += HandleMove;
            _inputManager.Player.Move.canceled += HandleMoveCancelled;
            
            _inputManager.Player.Jump.performed += HandleJump;

            _inputManager.Player.Ability1.performed += HandleAbility1Pressed;
            _inputManager.Player.Ability1.canceled += HandleAbility1Released;

            _inputManager.Player.Ability2.performed += HandleAbility2Pressed;
            _inputManager.Player.Ability2.canceled += HandleAbility2Release;
            
            _inputManager.Player.Ability3.performed += HandleAbility3Pressed;
            _inputManager.Player.Ability3.canceled += HandleAbility3Released;


        } 
        private void OnDisable()
        {
            _inputManager.Disable();
            
            _inputManager.Player.Move.performed -= HandleMove;
            _inputManager.Player.Move.canceled -= HandleMoveCancelled;
            
            _inputManager.Player.Jump.performed -= HandleJump;

            _inputManager.Player.Ability1.performed -= HandleAbility1Pressed;
            _inputManager.Player.Ability1.canceled -= HandleAbility1Released;
            
            _inputManager.Player.Ability2.performed -= HandleAbility2Pressed;
            _inputManager.Player.Ability2.canceled -= HandleAbility2Release;
        }
        #endregion

        #region Input Callbacks (Named Methods)

        private void HandleMove(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            _playerHandler.PlayerEvents.OnMove?.Invoke(input);
        }

        private void HandleMoveCancelled(InputAction.CallbackContext ctx)
        {
            _playerHandler.PlayerEvents.OnMove?.Invoke(Vector2.zero);
        }

        private void HandleJump(InputAction.CallbackContext ctx)
        {
            _playerHandler.PlayerEvents.OnJumpStarted?.Invoke();
        }

        private void HandleAbility1Pressed(InputAction.CallbackContext ctx)
        {
            // Slot 0 for Ability 1
            _playerHandler.PlayerEvents.OnAbilityPressed?.Invoke(0);
        }

        private void HandleAbility1Released(InputAction.CallbackContext ctx)
        {
            _playerHandler.PlayerEvents.OnAbilityReleased?.Invoke(0);
        }

        private void HandleAbility2Pressed(InputAction.CallbackContext ctx)
        {
            _playerHandler.PlayerEvents.OnAbilityPressed?.Invoke(1);
        }

        private void HandleAbility2Release(InputAction.CallbackContext ctx)
        {
            _playerHandler.PlayerEvents.OnAbilityReleased?.Invoke(1);
        }
        
        private void HandleAbility3Pressed(InputAction.CallbackContext ctx)
        {
            // Slot 0 for Ability 1
            _playerHandler.PlayerEvents.OnAbilityPressed?.Invoke(2);
        }

        private void HandleAbility3Released(InputAction.CallbackContext ctx)
        {
            _playerHandler.PlayerEvents.OnAbilityReleased?.Invoke(2);
        }
        

        #endregion
    }
}