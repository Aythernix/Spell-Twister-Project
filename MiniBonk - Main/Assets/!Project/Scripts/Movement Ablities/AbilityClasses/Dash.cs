using System.Collections;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    [CreateAssetMenu]
    public class Dash : ImpulseAbility
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
            _player.GetComponent<PlayerHandler>().PlayerEvents.OnDashStarted?.Invoke();
        }

        IEnumerator PerformDash()
        {
            dashActive = true;
            
            // Turns off players gravity to allow for smoother movement
            _rb.useGravity = false;
            
            // Position Before the dash
            Vector3 startPos = _rb.position;
            
            // Resets the players velocity to avoid interference
            _rb.linearVelocity = Vector3.zero;
            
            // Calls to prevent the player from moving
            _player.GetComponent<PlayerHandler>().PlayerEvents.PreventMovementInterference?.Invoke(true);
            
            // Gets the dash direction from the camera without Y
            var dashDir = new  Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z).normalized;
    
            // Applies the initial force
            _rb.AddForce(dashDir * _dashForce, ForceMode.Impulse);
    
            // Dash keeps going until it hits a certain distance from the start, or hits an object
            while (Vector3.Distance(startPos, _rb.position) < _maxDashDistance)
            {
                if (_rb.linearVelocity.sqrMagnitude < 0.5f) break; 
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
            
            // Allows player movement again
            _player.GetComponent<PlayerHandler>().PlayerEvents.PreventMovementInterference?.Invoke(false);
            
            _player.GetComponent<PlayerHandler>().PlayerEvents.OnDashFinished?.Invoke();
        }
    }
}

