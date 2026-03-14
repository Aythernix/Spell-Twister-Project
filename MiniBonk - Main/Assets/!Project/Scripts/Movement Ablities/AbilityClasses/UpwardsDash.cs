using System.Collections;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    [CreateAssetMenu]
    public class UpwardsDash : ImpulseAbility
    {
        [Header("Dash Settings")]
        [SerializeField] private float _dashForce;
        [SerializeField] private float _maxDashDistance;
        

        private bool dashActive;

        private Camera _camera;
        
        protected override void Start()
        {
            _camera = Camera.main;
        }

        protected override void ApplyImpulse()
        {
            _rb.GetComponent<MonoBehaviour>().StartCoroutine(PerformDash());
            _player.GetComponent<PlayerHandler>().PlayerEvents.OnUpwardsDashStarted?.Invoke();
        }

        IEnumerator PerformDash()
        {
            // Debug.Log("Upwards Dash Started");
            
            var defaultConstraints = _rb.constraints;
            
            dashActive = true;
            
            // Turns off players gravity to allow for smoother movement
            _rb.useGravity = false;
            
            // Position Before the dash
            Vector3 startPos = _rb.position;
            
            // Calls to prevent the player from moving
            _player.GetComponent<PlayerHandler>().PlayerEvents.PreventMovementInterference?.Invoke(true);
            
            _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            
            // Gets the dash direction from the camera without Y
            var dashDir = Vector3.up;
            
            // Resets the players velocity to avoid interference
            _rb.linearVelocity = Vector3.zero;
            
            _rb.linearVelocity = dashDir * _dashForce;
            
            yield return new WaitForSeconds(0.01f);
            // Dash keeps going until it hits a certain distance from the start, or hits an object
            while (Vector3.Distance(startPos, _rb.position) < _maxDashDistance)
            {
                Debug.Log("Dash Distance: " + Vector3.Distance(startPos, _rb.position));
                Debug.Log($"Dash Magnitude {_rb.linearVelocity.magnitude}, {_rb.linearVelocity.magnitude < 0.5f}");
                Debug.Log($"Velocity {_rb.linearVelocity}");
                
                if (_rb.linearVelocity.magnitude < 0.5f)
                {
                    break;
                } 
                
                yield return null;
            }

            Debug.Log("Dash Distance Reached - Beginning Braking");
            
            // Increases drag to slow down
            _rb.linearDamping = 10f; 
            yield return new WaitForSeconds(0.15f); 

            // Cleans up after timer
            _rb.linearDamping = 0f;
            _rb.useGravity = true;
            _rb.linearVelocity = Vector3.zero;
            dashActive = false;
            _rb.constraints = defaultConstraints;
            
            // Allows player movement again
            _player.GetComponent<PlayerHandler>().PlayerEvents.PreventMovementInterference?.Invoke(false);
            
            _player.GetComponent<PlayerHandler>().PlayerEvents.OnUpwardsDashFinished?.Invoke();
        }
    }
}

