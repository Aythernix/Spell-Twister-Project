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
            
        } 
        private void OnDisable()
        {
            _inputManager.Disable();
            
            _inputManager.Player.Move.performed -= HandleMove;
            _inputManager.Player.Move.canceled -= HandleMoveCancelled;
            
            _inputManager.Player.Jump.performed -= HandleJump;

            _inputManager.Player.Ability1.performed -= HandleAbility1Pressed;
            _inputManager.Player.Ability1.canceled -= HandleAbility1Released;
        }
        #endregion

        #region Input Callbacks (Named Methods)

        private void HandleMove(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            _playerHandler.events.OnMove?.Invoke(input);
        }

        private void HandleMoveCancelled(InputAction.CallbackContext ctx)
        {
            _playerHandler.events.OnMove?.Invoke(Vector2.zero);
        }

        private void HandleJump(InputAction.CallbackContext ctx)
        {
            _playerHandler.events.OnJumpStarted?.Invoke();
        }

        private void HandleAbility1Pressed(InputAction.CallbackContext ctx)
        {
            // Slot 0 for Ability 1
            _playerHandler.events.OnAbilityPressed?.Invoke(0);
        }

        private void HandleAbility1Released(InputAction.CallbackContext ctx)
        {
            _playerHandler.events.OnAbilityReleased?.Invoke(0);
        }

        #endregion
    }
}