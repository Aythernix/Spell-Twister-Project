using System;
using UnityEngine;
using Core.Management;

namespace Minibonk.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Variables

        // Serialized Fields
        
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 10f;

        [Header("References")] [SerializeField] private CharacterController _characterController;
        
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
            _characterController.Move(new Vector3(_directionInput.x * _moveSpeed, 0, _directionInput.y * _moveSpeed));
        }

        #endregion

        #region Private Methods
        // TODO Write Code
        
        
        #endregion
    }
}
