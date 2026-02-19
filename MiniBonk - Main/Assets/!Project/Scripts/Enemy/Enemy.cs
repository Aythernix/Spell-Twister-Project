using System;
using Minibonk.Management;
using UnityEngine;
using UnityEngine.AI;

namespace MiniBonk.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header ("Settings")]
        public EnemyData data; // Scriptable Object containing the enemy settings
        public Transform player;
        private NavMeshAgent navMeshAgent; 
        
        protected int currentHealth;
        private float nextAttackTime;

        // Update is called once per frame
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
            
            currentHealth = data.health;
            ApplySettings();
        }

        public virtual void TakeDamage( int amount, GameObject instigator = null)
        {
            currentHealth -= amount;
            Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die(instigator);
            }
        }

        protected virtual void Die(GameObject instigator)
        {
            if (data != null)
            {
                GameManager.Instance.EventHandler.AddXp?.Invoke(data.xpValue);
            }

            Debug.Log($"{gameObject.name} has died.");
            GameManager.Instance.EventHandler.OnEnemyKilled(instigator, gameObject);
            Destroy(gameObject);
        }
        
        private void ApplySettings()
        {
            navMeshAgent.speed = data.speed;
            navMeshAgent.acceleration = data.acceleration;
            navMeshAgent.stoppingDistance = data.stoppingDistance;
        }

        protected virtual void Update()
        {
            navMeshAgent.SetDestination(player.position);
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= data.attackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack(); // This calls the specific attack logic (Melee, Ranged, etc.)

                    nextAttackTime = Time.time + data.attackCooldown;
                }
                     
                navMeshAgent.isStopped = true;
            }
            else
            {
                navMeshAgent.isStopped = false;
            }
        }
        protected virtual void FixedUpdate()
        {
        }

        protected abstract void Attack();
    }
}
