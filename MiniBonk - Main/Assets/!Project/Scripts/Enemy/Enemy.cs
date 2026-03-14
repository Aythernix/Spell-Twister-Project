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
        private float lastUpdateTime;
        private Vector3 lastPlayerPosition;
        private EnemyAnimations enemyAnimations;
        private const float UPDATE_INTERVAL = 0.1f; // Update 10 times per second instead of 60+

        // Update is called once per frame
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemyAnimations = GetComponentInChildren<EnemyAnimations>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform.root;
            currentHealth = data.health;
            ApplySettings();
            lastUpdateTime = Time.time;
            lastPlayerPosition = player.position;
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
                // Debug.Log($"{gameObject.name} has died. Worth {data.xpValue}");
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
            if (!isAlive) return;
            
            // Only update pathfinding if player moved significantly or enough time passed
            if (Time.time >= lastUpdateTime + UPDATE_INTERVAL || 
                Vector3.Distance(lastPlayerPosition, player.position) > 0.5f)
            {
                navMeshAgent.SetDestination(player.position);
                lastUpdateTime = Time.time;
                lastPlayerPosition = player.position;
            }
            
            // Use squared distance for comparison (faster than sqrt)
            float sqrDistance = (transform.position - player.position).sqrMagnitude;
            float sqrAttackRange = data.attackRange * data.attackRange;
            
            if (sqrDistance <= sqrAttackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack(); // This calls the specific attack logic (Melee, Ranged, etc.)
                    if (enemyAnimations != null)
                        enemyAnimations.CheckForAttacking();

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
