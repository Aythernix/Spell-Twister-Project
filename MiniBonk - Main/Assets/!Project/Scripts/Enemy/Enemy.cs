using System;
using Minibonk.Enemy;
using Minibonk.Management;
using Unity.VisualScripting;
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
        // Update is called once per frame
        protected virtual void Start()
        {
            
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform.root;
            playerRigidbody = player.GetComponent<Rigidbody>();
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
            
            navMeshAgent.speed = data.speed;
            navMeshAgent.acceleration = data.acceleration;
            navMeshAgent.stoppingDistance = data.stoppingDistance;
        }

        protected virtual void Update()
        {
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
        protected virtual void FixedUpdate()
        {
            Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, data.seperationRadius);

            foreach (var other in nearbyEnemies)
            {

                if (other.gameObject != gameObject && other.CompareTag("Enemy"))
                {
                    Vector3 pushDirection = transform.position - other.transform.position;
                    transform.position += pushDirection.normalized * data.seperationAmount * Time.fixedDeltaTime;
                }
            }
            
        }

        protected abstract void Attack();
    }
}
