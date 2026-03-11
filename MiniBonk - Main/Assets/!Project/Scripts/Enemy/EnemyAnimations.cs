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
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            CheckForMovement();
        }

        private void CheckForMovement()
        {
            if (!_navMeshAgent.isStopped)
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
            _animator.SetTrigger("IsAttacking");
        }
    }
}
