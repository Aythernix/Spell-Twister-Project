using System;
using System.Collections;
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
        
        [Header("Hit Visuals")]
        [SerializeField] protected SpriteRenderer overheadIcon;
        [SerializeField] public Renderer[] model; //What is allowed to be changed by propBlock, turned public for frost effect
        [SerializeField] protected ParticleSystem blood;
        [SerializeField] protected ParticleSystem deathBurst;
        [SerializeField] protected float hitEffectDuration = 0.2f;

        public MaterialPropertyBlock propBlock; //Used  to change color of enemy
        private Coroutine hitEffectCoroutine; // Used to display Particles
        private static readonly int ColorProperty = Shader.PropertyToID("_BaseColor");
        
        protected AudioSource _enemyAudioSource;
        
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
            _enemyAudioSource = GetComponent<AudioSource>();
            
            reactionDelay = data.reactionDelay + UnityEngine.Random.Range(0.1f, 0.3f);
            
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

            GameManager.Instance.EventHandler.OnEnemyDamaged?.Invoke(amount, gameObject);
            
            if (currentHealth <= 0)
            {
                Die(instigator);
                return;
            }
            
            TriggerHitFeedback();
        }
                     
        
        private void TriggerHitFeedback()
        {
            if (blood != null)
            {
                Debug.Log($"Playing particles for {name}");
                blood.Play();
            }
            if (hitEffectCoroutine != null) StopCoroutine(hitEffectCoroutine);
            hitEffectCoroutine = StartCoroutine(hitEffectSequence());
        }

        private IEnumerator hitEffectSequence()
        {
            if (propBlock == null) propBlock = new MaterialPropertyBlock();
            if (overheadIcon != null) overheadIcon.enabled = true;

            propBlock.SetColor(ColorProperty, Color.red);
            foreach (var rend in model)
            {
                rend.SetPropertyBlock(propBlock);
            }
            yield return new WaitForSeconds(hitEffectDuration);
            
            if (overheadIcon != null) overheadIcon.enabled = false;
            
            propBlock.Clear();
            foreach (var rend in model)
            {
                rend.SetPropertyBlock(propBlock);
            }
        }

        public virtual void Die(GameObject instigator)
        {
            isAlive = false;

            if (hitEffectCoroutine != null) //Safeguard to prevent glitching with the Death effect
            {
                StopCoroutine(hitEffectCoroutine);
            }

            if (deathBurst != null)
            {
                deathBurst.transform.SetParent(null);
                deathBurst.Play();
            }
            
            //Unregister AI
            EnemyProcessor.UnregisterEnemy(this);
            if (navMeshAgent != null) navMeshAgent.enabled = false;
            if (TryGetComponent<Collider>(out var collider)) collider.enabled = false;
            
            //Handles XP and drops
            if (data != null)
            {
                if (instigator != null)
                {
                    GameManager.Instance.EventHandler.AddXp?.Invoke((int)(data.xpValue * GameManager.Instance.ChaosMultiplier()));
                    if (GameManager.Instance.Probability(data.chanceOfDroppingHealth * GameManager.Instance.ChaosMultiplier()))
                    {
                        Instantiate(data.healthDrop, transform.position, Quaternion.identity);
                        
                        
                    }
                }
                GameManager.Instance.EventHandler.OnEnemyKilled?.Invoke(instigator, gameObject);
                
            }
            
            
            StartCoroutine(DeathTransition());
        }

        private IEnumerator DeathTransition()
        {
            if (propBlock == null) propBlock = new MaterialPropertyBlock();
            propBlock.SetColor(ColorProperty, Color.red);
            foreach (var rend in model)
            {
                rend.SetPropertyBlock(propBlock);
            }

            float duration = 0.5f;
            float elapsed = 0f;
            Vector3 startScale = transform.localScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                transform.position += Vector3.down *(Time.deltaTime * 0.5f);
                yield return null;
            }
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

        [SerializeField] float reactionDelay = 0.2f;
        private float lastReactionTime;
        
        public void UpdatePathfinding(Vector3 realPlayerPosition)
        {
            Vector3 toPlayer = realPlayerPosition - transform.position;
            float sqrDistance = toPlayer.sqrMagnitude;
            if (!isAlive || !navMeshAgent.isOnNavMesh) return;

            if (Time.time >= lastReactionTime + reactionDelay)
            {
                lastReactionTime = Time.time;

                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-1f, 1f),
                    0,
                    UnityEngine.Random.Range(-1f, 1f)
                ) * 0.5f;

                Vector3 finalTarget = realPlayerPosition + randomOffset;

                if (playerRigidbody != null && data.predictionTime > 0)
                {
                    Vector3 lead = playerRigidbody.linearVelocity * (data.predictionTime * data.predictionIntensity);
                    Vector3 error = new Vector3(UnityEngine.Random.Range(-1f,1f), 0, UnityEngine.Random.Range(-1f,1f));
                    float distance = Mathf.Sqrt(sqrDistance);
                    float maxLead = distance * 0.9f;

                    if (lead.sqrMagnitude > maxLead * maxLead)
                    {
                        lead = lead.normalized * maxLead;
                    }
                    finalTarget += lead + error;
                }

                navMeshAgent.SetDestination(finalTarget);
            }
            
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