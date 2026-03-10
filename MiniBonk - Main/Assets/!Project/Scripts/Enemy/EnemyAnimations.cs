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

        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();

            
        }

        // Update is called once per frame
        void Update()
        {
            CheckForMovement();
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
            Debug.Log($"Speed: {_navMeshAgent.velocity.magnitude} | IsMoving: {_animator.GetBool("IsMoving")} ");
            
        }

        public void CheckForAttacking()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("IsAttacking");
                Debug.Log("!!!Attack Triggered!!!");
            }
               
        }


    }
}
