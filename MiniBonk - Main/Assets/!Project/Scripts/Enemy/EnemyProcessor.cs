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
        
        private static EnemyProcessor _instance;
        public static EnemyProcessor Instance => _instance;
        
        private List<MiniBonk.Enemy.Enemy> _activeEnemies = new List<MiniBonk.Enemy.Enemy>();
        private List<EnemyAnimations> _enemyAnimations = new List<EnemyAnimations>();
        private float _lastProcessTime;
        private int _currentProcessIndex;
        
        // Cached references
        private Transform _playerTransform;
        private Vector3 _lastPlayerPosition;
        #endregion
        
        #region Unity Methods
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        private void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            _lastPlayerPosition = _playerTransform?.position ?? Vector3.zero;
            _lastProcessTime = Time.time;
        }
        
        private void Update()
        {
            if (Time.time >= _lastProcessTime + _updateInterval)
            {
                ProcessAllEnemies();
                _lastProcessTime = Time.time;
            }
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Register an enemy for centralized processing
        /// </summary>
        public static void RegisterEnemy(MiniBonk.Enemy.Enemy enemy)
        {
            if (_instance != null && enemy != null && !_instance._activeEnemies.Contains(enemy))
            {
                _instance._activeEnemies.Add(enemy);
                
                // Also register animations if available
                var animations = enemy.GetComponentInChildren<EnemyAnimations>();
                if (animations != null && !_instance._enemyAnimations.Contains(animations))
                {
                    _instance._enemyAnimations.Add(animations);
                }
            }
        }
        
        /// <summary>
        /// Unregister an enemy when destroyed
        /// </summary>
        public static void UnregisterEnemy(MiniBonk.Enemy.Enemy enemy)
        {
            if (_instance != null && enemy != null)
            {
                _instance._activeEnemies.Remove(enemy);
                
                var animations = enemy.GetComponentInChildren<EnemyAnimations>();
                if (animations != null)
                {
                    _instance._enemyAnimations.Remove(animations);
                }
            }
        }
        #endregion
        
        #region Private Methods
        /// <summary>
        /// Process all enemies in batches for optimal performance
        /// </summary>
        private void ProcessAllEnemies()
        {
            if (_activeEnemies.Count == 0) return;
            
            // Check if player moved significantly
            bool playerMoved = _playerTransform != null && 
                             Vector3.Distance(_lastPlayerPosition, _playerTransform.position) > 0.5f;
            
            if (playerMoved)
            {
                _lastPlayerPosition = _playerTransform.position;
            }
            
            // Process enemies in batches to avoid frame spikes
            int processedCount = 0;
            int startIndex = _currentProcessIndex;
            
            for (int i = 0; i < _activeEnemies.Count && processedCount < _maxEnemiesPerFrame; i++)
            {
                int index = (startIndex + i) % _activeEnemies.Count;
                var enemy = _activeEnemies[index];
                
                if (enemy != null && enemy.isAlive)
                {
                    ProcessEnemy(enemy, playerMoved);
                    processedCount++;
                }
                else if (enemy == null || !enemy.isAlive)
                {
                    // Remove dead/null enemies
                    _activeEnemies.RemoveAt(index);
                    if (index < _enemyAnimations.Count)
                    {
                        _enemyAnimations.RemoveAt(index);
                    }
                    i--; // Adjust index since we removed an item
                }
            }
            
            // Update process index for next frame
            _currentProcessIndex = (startIndex + processedCount) % _activeEnemies.Count;
            
            // Process animations separately
            ProcessAnimations();
        }
        
        /// <summary>
        /// Process individual enemy logic
        /// </summary>
        private void ProcessEnemy(MiniBonk.Enemy.Enemy enemy, bool playerMoved)
        {
            // Only update pathfinding if player moved significantly or enough time passed
            if (playerMoved || Time.time >= enemy.GetLastUpdateTime() + enemy.GetUpdateInterval())
            {
                enemy.UpdatePathfinding(_playerTransform.position);
                enemy.SetLastUpdateTime(Time.time);
            }
            
            // Use squared distance for comparison (faster than sqrt)
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
        
        /// <summary>
        /// Process all enemy animations in batch
        /// </summary>
        private void ProcessAnimations()
        {
            for (int i = 0; i < _enemyAnimations.Count; i++)
            {
                var anim = _enemyAnimations[i];
                if (anim != null)
                {
                    anim.ProcessAnimation();
                }
                else
                {
                    _enemyAnimations.RemoveAt(i);
                    i--;
                }
            }
        }
        #endregion
    }
}
