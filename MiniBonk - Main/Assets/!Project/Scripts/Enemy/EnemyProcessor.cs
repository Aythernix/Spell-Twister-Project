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
        
        // Separation optimization - single array for all enemies
        private Collider[] _hitColliders = new Collider[50];
        
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
        /// Process enemies in small chunks every frame for smooth performance
        /// </summary>
        private void ProcessEnemyChunk()
        {
            if (_activeEnemies.Count == 0) return;
            
            // Process only a small number of enemies per frame
            int enemiesPerFrame = 5; // Much smaller than before
            int processedCount = 0;
            
            // Check if player moved significantly (less frequent check)
            bool playerMoved = _playerTransform != null && 
                             Vector3.Distance(_lastPlayerPosition, _playerTransform.position) > 1.0f;
            
            if (playerMoved)
            {
                _lastPlayerPosition = _playerTransform.position;
            }
            
            // Process enemies in small chunks
            for (int i = 0; i < _activeEnemies.Count && processedCount < enemiesPerFrame; i++)
            {
                int index = (_currentProcessIndex + i) % _activeEnemies.Count;
                var enemy = _activeEnemies[index];
                
                if (enemy != null && enemy.isAlive)
                {
                    // Only update pathfinding less frequently
                    if (playerMoved || Time.time >= enemy.GetLastUpdateTime() + enemy.GetUpdateInterval())
                    {
                        enemy.UpdatePathfinding(_playerTransform.position);
                        enemy.SetLastUpdateTime(Time.time);
                    }
                    
                    // Check combat less frequently
                    if (Time.time % 0.2f < Time.deltaTime) // Every ~200ms
                    {
                        ProcessCombatLogic(enemy);
                    }
                    
                    // Process separation even less frequently
                    if (Time.time % 0.5f < Time.deltaTime) // Every ~500ms
                    {
                        ProcessSeparation(enemy);
                    }
                    
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
            _currentProcessIndex = (_currentProcessIndex + processedCount) % _activeEnemies.Count;
            
            // Process animations separately but also in chunks
            if (Time.time % 0.1f < Time.deltaTime) // Every ~100ms
            {
                ProcessAnimationChunk();
            }
        }
        
        /// <summary>
        /// Process combat logic separately for better performance
        /// </summary>
        private void ProcessCombatLogic(MiniBonk.Enemy.Enemy enemy)
        {
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
        /// Process animations in small chunks
        /// </summary>
        private void ProcessAnimationChunk()
        {
            int animationsPerFrame = 3;
            int processed = 0;
            
            for (int i = 0; i < _enemyAnimations.Count && processed < animationsPerFrame; i++)
            {
                var anim = _enemyAnimations[i];
                if (anim != null)
                {
                    anim.ProcessAnimation();
                    processed++;
                }
                else
                {
                    _enemyAnimations.RemoveAt(i);
                    i--;
                }
            }
        }
        
        /// <summary>
        /// Process enemy separation using centralized collision detection
        /// </summary>
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
