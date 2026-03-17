using System;
using UnityEngine;
using UnityEngine.AI;
using Minibonk.Management;

namespace MiniBonk.Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Settings")]
        public EnemyData data; 
        public Transform player;
        public bool isAlive = true;
        
        private NavMeshAgent navMeshAgent;
        protected int currentHealth;
        private float nextAttackTime;
        private Rigidbody playerRigidbody;
        private float lastUpdateTime;
        
        private const float UPDATE_INTERVAL = 0.1f;
        
        // Cached variables for optimization
        private float _normalSpeed;
        private float _boostedSpeed;
        private bool _isBoosted; 
        private const float BOOST_DISTANCE_SQR = 15f * 15f; // 225f

        private void OnEnable()
        {
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease += ChaosMultiplier;
        }
        private void OnDisable()
        {
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease -= ChaosMultiplier;
        }
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            if (player == null) 
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) player = playerObj.transform.root;
            }
            
            if (player != null)
            {
                playerRigidbody = player.GetComponent<Rigidbody>();
            }
            
            currentHealth = (int)(data.health * GameManager.Instance.ChaosMultiplier());
            ApplySettings();
            
            lastUpdateTime = Time.time;
            
            EnemyProcessor.RegisterEnemy(this);
        }

        public virtual void TakeDamage(int amount, GameObject instigator = null)
        {
            if (!isAlive) return; // Prevent taking damage after death

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Die(instigator);
            }
        }

        protected virtual void Die(GameObject instigator)
        {
            isAlive = false;
            
            EnemyProcessor.UnregisterEnemy(this);
            
            if (data != null)
            {
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
            _normalSpeed = data.speed + UnityEngine.Random.Range(-data.speedVariance, data.speedVariance);
            _boostedSpeed = _normalSpeed * 1.5f; // Cache the math here once
            
            navMeshAgent.speed = _normalSpeed;
            navMeshAgent.acceleration = data.acceleration;
            navMeshAgent.stoppingDistance = data.stoppingDistance;
            _isBoosted = false;
        }

        private void ChaosMultiplier(int lvl)
        {
            _normalSpeed *= GameManager.Instance.ChaosMultiplier();
            _boostedSpeed *= GameManager.Instance.ChaosMultiplier();
        }
        
        public float GetLastUpdateTime() => lastUpdateTime;
        public void SetLastUpdateTime(float time) => lastUpdateTime = time;
        public float GetUpdateInterval() => UPDATE_INTERVAL;
        public float GetAttackRange() => data.attackRange;
        public float GetNextAttackTime() => nextAttackTime;
        public void SetNextAttackTime(float time) => nextAttackTime = time;
        public float GetAttackCooldown() => data.attackCooldown;
        
        public void UpdatePathfinding(Vector3 realPlayerPosition)
        {
            if (!isAlive || !navMeshAgent.isOnNavMesh) return;
            
            // 1. Calculate Squared Distance (Extremely fast)
            Vector3 toPlayer = realPlayerPosition - transform.position;
            float sqrDistance = toPlayer.sqrMagnitude;
            
            Vector3 finalTarget = realPlayerPosition;

            // 2. Prediction Logic
            if (playerRigidbody != null && data.predictionTime > 0)
            {
                Vector3 lead = playerRigidbody.linearVelocity * (data.predictionTime * data.predictionIntensity);

                // We only do the heavy Sqrt calculation if prediction is actually active
                float distance = Mathf.Sqrt(sqrDistance);
                float maxLead = distance * 0.9f;

                if (lead.sqrMagnitude > maxLead * maxLead)
                {
                    lead = lead.normalized * maxLead;
                }
                finalTarget += lead;
            }
            
            navMeshAgent.SetDestination(finalTarget);
            
            bool shouldBoost = sqrDistance > BOOST_DISTANCE_SQR;
            if (shouldBoost != _isBoosted)
            {
                _isBoosted = shouldBoost;
                navMeshAgent.speed = _isBoosted ? _boostedSpeed : _normalSpeed;
            }
        }
        
        public void StopMovement() => navMeshAgent.isStopped = true;
        public void ResumeMovement() => navMeshAgent.isStopped = false;

        public abstract void Attack();
    }
}