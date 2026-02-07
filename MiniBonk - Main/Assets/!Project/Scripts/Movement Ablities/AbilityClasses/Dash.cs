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
        }

        IEnumerator PerformDash()
        {
            dashActive = true;
            _rb.useGravity = false;
    
            Vector3 startPos = _rb.position;
            _rb.linearVelocity = Vector3.zero;
            
            _player.GetComponent<PlayerHandler>().Events.PreventMovementInterference?.Invoke(true);
            
            var dashDir = new  Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z).normalized;
    
            // 1. The Initial Burst
            _rb.AddForce(dashDir * _dashForce, ForceMode.Impulse);
    
            // 2. Travel Phase
            while (Vector3.Distance(startPos, _rb.position) < _maxDashDistance)
            {
                if (_rb.linearVelocity.sqrMagnitude < 0.5f) break; 
                yield return null;
            }

            Debug.Log("Dash Distance Reached - Beginning Braking");
            
            _rb.linearDamping = 10f; 
            yield return new WaitForSeconds(0.15f); 

            // 4. Cleanup
            _rb.linearDamping = 0f;
            _rb.useGravity = true;
            _rb.linearVelocity = Vector3.zero;
            dashActive = false;
            _player.GetComponent<PlayerHandler>().Events.PreventMovementInterference?.Invoke(false);
        }
    }
}

