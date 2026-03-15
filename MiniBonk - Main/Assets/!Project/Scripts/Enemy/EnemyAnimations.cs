using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Minibonk.Enemy
{
    public class EnemyAnimations : MonoBehaviour
    {
        #region Variables

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private Vector3 lastVelocity;
        private bool wasMoving;
        private const float UPDATE_INTERVAL = 0.1f; // Check 10 times per second
        private float lastUpdateTime;

        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            lastVelocity = _navMeshAgent.velocity;
            wasMoving = false;
            lastUpdateTime = Time.time;
        }

        private void Update()
        {
            // Update method disabled - now handled by EnemyProcessor
        }
        
        /// <summary>
        /// Process animation logic - called by EnemyProcessor
        /// </summary>
        public void ProcessAnimation()
        {
            if (_animator == null) return;
            
            if (_navMeshAgent.velocity.magnitude > 0.1f)
            {
                _animator.SetBool("IsMoving", true);
            }
            else
            {
                _animator.SetBool("IsMoving", false);
            }
        }

        private void CheckForMovement()
        {
            if (_animator == null) return;
            
            if (_navMeshAgent.velocity.magnitude > 0.1f)
            {
                _animator.SetBool("IsMoving", true);
            }
            else
            {
                _animator.SetBool("IsMoving", false);
            }
        }

        public void CheckForAttacking()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("IsAttacking");
                // Debug.Log($"!!!Attack Triggered!!!");
            }
               
        }
    }
}
