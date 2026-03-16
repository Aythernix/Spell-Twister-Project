using System.Collections.Generic;
using UnityEngine;
using Minibonk.Enemy;
using Minibonk.Management;

namespace Minibonk.Management
{
    /// <summary>
    /// Centralized enemy processor that handles all enemy logic in batches
    /// Instead of hundreds of individual Update() calls, this processes all enemies efficiently
    /// </summary>
    public class EnemyProcessor : MonoBehaviour
    {
        #region Variables
        [Header("Processing Settings")]
        [SerializeField] private float _updateInterval = 0.1f; // Process 10 times per second
        [SerializeField] private int _maxEnemiesPerFrame = 50; // Process in batches
        
        private List<MiniBonk.Enemy.Enemy> _activeEnemies = new List<MiniBonk.Enemy.Enemy>();
        private List<EnemyAnimations> _enemyAnimations = new List<EnemyAnimations>();
        private float _lastProcessTime;
        private int _currentProcessIndex;
        
        // Separation optimization - single array for all enemies
        private Collider[] _hitColliders = new Collider[50];
        
        // Animation processing optimization
        private int _currentAnimationIndex;
        
        // Cached references
        private Transform _playerTransform;
        private Vector3 _lastPlayerPosition;
        #endregion
        
        #region Unity Methods
        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            _lastPlayerPosition = _playerTransform?.position ?? Vector3.zero;
            _lastProcessTime = Time.time;
        }
        
        private void Update()
        {
            // Process in smaller chunks every frame instead of large batches every 0.1s
            ProcessEnemyChunk();
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Register an enemy for centralized processing
        /// </summary>
        public static void RegisterEnemy(MiniBonk.Enemy.Enemy enemy)
        {
            var processor = FindFirstObjectByType<EnemyProcessor>();
            if (processor != null && enemy != null && !processor._activeEnemies.Contains(enemy))
            {
                processor._activeEnemies.Add(enemy);
                
                // Also register animations if available
                var animations = enemy.GetComponentInChildren<EnemyAnimations>();
                if (animations != null && !processor._enemyAnimations.Contains(animations))
                {
                    processor._enemyAnimations.Add(animations);
                }
            }
        }
        
        /// <summary>
        /// Unregister an enemy when destroyed
        /// </summary>
        public static void UnregisterEnemy(MiniBonk.Enemy.Enemy enemy)
        {
            var processor = FindFirstObjectByType<EnemyProcessor>();
            if (processor != null && enemy != null)
            {
                processor._activeEnemies.Remove(enemy);
                
                var animations = enemy.GetComponentInChildren<EnemyAnimations>();
                if (animations != null)
                {
                    processor._enemyAnimations.Remove(animations);
                }
            }
        }
        #endregion
        
        #region Private Methods
        private void ProcessEnemyChunk()
        {
            if (_activeEnemies.Count == 0) return;
            
            int enemiesPerFrame = 5;
            int processedCount = 0;
            
            bool playerMoved = _playerTransform != null && 
                             Vector3.Distance(_lastPlayerPosition, _playerTransform.position) > 1.0f;
            
            if (playerMoved)
            {
                _lastPlayerPosition = _playerTransform.position;
            }
            
            for (int i = 0; i < _activeEnemies.Count && processedCount < enemiesPerFrame; i++)
            {
                int index = (_currentProcessIndex + i) % _activeEnemies.Count;
                var enemy = _activeEnemies[index];
                
                if (enemy != null && enemy.isAlive)
                {
                    if (playerMoved || Time.time >= enemy.GetLastUpdateTime() + enemy.GetUpdateInterval())
                    {
                        enemy.UpdatePathfinding(_playerTransform.position);
                        enemy.SetLastUpdateTime(Time.time);
                    }
                    
                    if (Time.time % 0.2f < Time.deltaTime)
                    {
                        ProcessCombatLogic(enemy);
                    }
                    
                    if (Time.time % 0.5f < Time.deltaTime)
                    {
                        ProcessSeparation(enemy);
                    }
                    
                    processedCount++;
                }
                else if (enemy == null || !enemy.isAlive)
                {
                    _activeEnemies.RemoveAt(index);
                    if (index < _enemyAnimations.Count)
                    {
                        _enemyAnimations.RemoveAt(index);
                    }
                    i--;
                }
            }
            
            _currentProcessIndex = (_currentProcessIndex + processedCount) % _activeEnemies.Count;
            
            ProcessAnimationChunk();
        }
        
        private void ProcessCombatLogic(MiniBonk.Enemy.Enemy enemy)
        {
            float sqrDistance = (enemy.transform.position - _playerTransform.position).sqrMagnitude;
            float sqrAttackRange = enemy.GetAttackRange() * enemy.GetAttackRange();
            
            if (sqrDistance <= sqrAttackRange)
            {
                if (Time.time >= enemy.GetNextAttackTime())
                {
                    enemy.Attack();
                    enemy.SetNextAttackTime(Time.time + enemy.GetAttackCooldown());
                    enemy.StopMovement();
                }
            }
            else
            {
                enemy.ResumeMovement();
            }
        }
        
        private void ProcessAnimationChunk()
        {
            if (_enemyAnimations.Count == 0) return;
            
            int animationsPerFrame = 5;
            int processed = 0;
            
            for (int i = 0; i < _enemyAnimations.Count && processed < animationsPerFrame; i++)
            {
                int index = (_currentAnimationIndex + i) % _enemyAnimations.Count;
                var anim = _enemyAnimations[index];
                
                if (anim != null && anim.enabled)
                {
                    anim.ProcessAnimation();
                    processed++;
                }
                else if (anim == null)
                {
                    _enemyAnimations.RemoveAt(index);
                    i--;
                    if (index < _currentAnimationIndex)
                        _currentAnimationIndex--;
                }
            }
            
            _currentAnimationIndex = (_currentAnimationIndex + processed) % _enemyAnimations.Count;
        }
        
        private void ProcessSeparation(MiniBonk.Enemy.Enemy enemy)
        {
            if (enemy.data == null) return;
            
            int hitCount = UnityEngine.Physics.OverlapSphereNonAlloc(
                enemy.transform.position, 
                enemy.data.seperationRadius, 
                _hitColliders
            );

            for (int i = 0; i < hitCount; i++)
            {
                var other = _hitColliders[i];
                if (other.gameObject != enemy.gameObject && other.CompareTag("Enemy"))
                {
                    Vector3 pushDirection = enemy.transform.position - other.transform.position;
                    enemy.transform.position += pushDirection.normalized * enemy.data.seperationAmount * Time.fixedDeltaTime;
                }
            }
        }

        #endregion
    }
}
