using System;
using Minibonk.Management;
using UnityEngine;

namespace Minibonk.Management
{
    public class InputHandler : PlayerSystem
    {
        #region Variables

        [SerializeField] private InputManager _inputManager;
        #endregion
       
        protected override void Awake()
        {
            base.Awake();
            _inputManager = new InputManager();
            
            _inputManager.Player.Move.performed += ctx => _playerHandler.events.OnMove?.Invoke(ctx.ReadValue<Vector2>());
            _inputManager.Player.Move.canceled += _ => _playerHandler.events.OnMove?.Invoke(Vector2.zero);
            
            _inputManager.Player.Jump.performed += _ => _playerHandler.events.OnJumpStarted?.Invoke();

            _inputManager.Player.Ability1.performed += _ => _playerHandler.events.OnAbilityPressed?.Invoke(1);
            _inputManager.Player.Ability1.canceled += _ => _playerHandler.events.OnAbilityReleased?.Invoke(1);
        }

        private void OnEnable()
        {
            _inputManager.Enable();
        }

        private void OnDisable()
        {
            _inputManager.Disable();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
