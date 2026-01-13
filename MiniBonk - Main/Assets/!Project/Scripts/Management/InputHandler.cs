using System;
using Minibonk.Management;
using UnityEngine;

namespace Minibonk.Management
{
    public class InputHandler : MonoBehaviour
    {
        #region Variables

        [SerializeField] private InputManager _inputManager;
        #endregion

        #region Events

        public Action<Vector2> OnMove;
        public Action OnJumpStarted;
        public Action<int> OnAbilityPressed;
        public Action<int> OnAbilityReleased;

        #endregion
       
        void Awake()
        {
            _inputManager = new InputManager();
            
            _inputManager.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
            _inputManager.Player.Move.canceled += _ => OnMove?.Invoke(Vector2.zero);
            
            _inputManager.Player.Jump.performed += _ => OnJumpStarted?.Invoke();
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
