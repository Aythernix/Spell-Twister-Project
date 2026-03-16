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
        private Rigidbody playerRigidbody;
        private float lastUpdateTime;
        private Vector3 lastPlayerPosition;
        private const float UPDATE_INTERVAL = 0.1f; // Update 10 times per second instead of 60+

        // Update is called once per frame
        protected virtual void Start()
        {
            
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform.root;
            currentHealth = (int)(data.health * GameManager.Instance.ChaosMultiplier());
            playerRigidbody = player.GetComponent<Rigidbody>();
            ApplySettings();
            lastUpdateTime = Time.time;
            lastPlayerPosition = player.position;
            
            // Register with centralized processor
            EnemyProcessor.RegisterEnemy(this);
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
            
            // Unregister from centralized processor
            EnemyProcessor.UnregisterEnemy(this);
            
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
            float randomSpeed = data.speed + UnityEngine.Random.Range(-data.speedVariance, data.speedVariance);
            
            navMeshAgent.speed = randomSpeed;
            navMeshAgent.acceleration = data.acceleration;
            navMeshAgent.stoppingDistance = data.stoppingDistance;
        }

        protected virtual void Update()
        {
            // Update method disabled - now handled by EnemyProcessor
            
        }
        

        // Public methods for EnemyProcessor access
        public float GetLastUpdateTime() => lastUpdateTime;
        public void SetLastUpdateTime(float time) => lastUpdateTime = time;
        public float GetUpdateInterval() => UPDATE_INTERVAL;
        public float GetAttackRange() => data.attackRange;
        public float GetNextAttackTime() => nextAttackTime;
        public void SetNextAttackTime(float time) => nextAttackTime = time;
        public float GetAttackCooldown() => data.attackCooldown;
        
        public void UpdatePathfinding(Vector3 targetPosition)
        {
            navMeshAgent.SetDestination(targetPosition);
            lastPlayerPosition = targetPosition;
            if (!isAlive) return;
            
            Vector3 targetPos = player.position;
            if (playerRigidbody != null)
            {
                targetPos += playerRigidbody.linearVelocity * data.predictionTime;
            }
            navMeshAgent.SetDestination(targetPos);
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > 15f)
            {
                navMeshAgent.speed = data.speed * 1.5f; //Catch up to player if far
            }
            else
            {
                navMeshAgent.speed = data.speed;
            }
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
        
        private Collider[] hitColliders = new Collider[10];
        
        public void StopMovement() => navMeshAgent.isStopped = true;
        public void ResumeMovement() => navMeshAgent.isStopped = false;
        protected virtual void FixedUpdate()
        {
            // Separation logic moved to EnemyProcessor for better performance
        }

        public abstract void Attack();
    }
}
