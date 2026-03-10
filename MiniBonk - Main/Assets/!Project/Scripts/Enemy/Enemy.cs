using System;
using Minibonk.Enemy;
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
        public bool isAlive = true;
        
        protected int currentHealth;
        private float nextAttackTime;

        // Update is called once per frame
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform.root;
            currentHealth = data.health;
            ApplySettings();
        }

        public virtual void TakeDamage( int amount, GameObject instigator = null)
        {
            currentHealth -= amount;
            // Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}");

            if (currentHealth <= 0 && isAlive)
            {
                Die(instigator);
            }
        }

        protected virtual void Die(GameObject instigator)
        {
            isAlive = false;
            if (data != null)
            {
                Debug.Log($"{gameObject.name} has died. Worth {data.xpValue}");
                GameManager.Instance.EventHandler.AddXp?.Invoke((int)(data.xpValue * GameManager.Instance.ChaosMultiplier()));
                if (GameManager.Instance.Probability(data.chanceOfDroppingHealth * GameManager.Instance.ChaosMultiplier()))
                {
                    Instantiate(data.healthDrop, transform.position, Quaternion.identity);
                }
            }
            
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
                    GetComponentInChildren<EnemyAnimations>().CheckForAttacking();

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
