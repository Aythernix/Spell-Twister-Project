using System;
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

        // Update is called once per frame
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHealth = data.health;
            ApplySettings();
        }

        public virtual void TakeDamage(int amount)
        {
            currentHealth -= amount;
            Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }
        
        private void ApplySettings()
        {
            navMeshAgent.speed = data.speed;
            navMeshAgent.acceleration = data.acceleration;
            navMeshAgent.stoppingDistance = data.stoppingDistance;
        }
        protected virtual void FixedUpdate()
        {
            navMeshAgent.SetDestination(player.position);
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= data.attackRange)
            {
                Attack(); // This calls the specific attack logic (Melee, Ranged, etc.)
            }
        }
        public abstract void Attack();
    }
}
