using System.Collections.Generic;
using UnityEngine;
using Minibonk.Enemy;

namespace Minibonk.Management
{
    public class EnemyProcessor : MonoBehaviour
    {
        #region Variables
        [Header("Optimization Settings")]
        [SerializeField] private int _pathfindingProcessedPerFrame = 20;
        [SerializeField] private int _animationsProcessedPerFrame = 15;
        
        [Header("Physics Settings")]
        [SerializeField] private LayerMask _enemyLayer; 

        public static EnemyProcessor Instance { get; private set; }
        
        private List<MiniBonk.Enemy.Enemy> _activeEnemies = new List<MiniBonk.Enemy.Enemy>();
        private List<EnemyAnimations> _enemyAnimations = new List<EnemyAnimations>();
        
        private int _currentPathIndex = 0;
        private int _currentAnimIndex = 0;
        
        private Collider[] _hitColliders = new Collider[15];
        
        private Transform _playerTransform;
        private Vector3 _lastPlayerPosition;
        #endregion
        
        #region Unity Methods
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            var playerGo = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = playerGo != null ? playerGo.transform : null;
            _lastPlayerPosition = _playerTransform != null ? _playerTransform.position : Vector3.zero;
        }
        
        private void Update()
        {
            if (_activeEnemies.Count == 0 || _playerTransform == null) return;

            bool playerMoved = Vector3.Distance(_lastPlayerPosition, _playerTransform.position) > 1.0f;
            if (playerMoved) _lastPlayerPosition = _playerTransform.position;

            // 1. ALL ENEMIES: Distance, Attacks, and Separation (Cheap Math)
            ProcessAllFastLogic();

            // 2. CHUNKED ENEMIES: NavMesh Routing (Heavy Math)
            ProcessPathfindingChunk(playerMoved);

            // 3. CHUNKED ANIMATIONS
            ProcessAnimationChunk();
        }
        #endregion
        
        #region Public Methods
        public static void RegisterEnemy(MiniBonk.Enemy.Enemy enemy)
        {
            if (Instance == null || enemy == null) return;
            
            Instance._activeEnemies.Add(enemy);
            
            var anim = enemy.GetComponentInChildren<EnemyAnimations>();
            if (anim != null) Instance._enemyAnimations.Add(anim);
        }
        
        public static void UnregisterEnemy(MiniBonk.Enemy.Enemy enemy)
        {
            if (Instance == null || enemy == null) return;
            
            Instance._activeEnemies.Remove(enemy);
            
            var anim = enemy.GetComponentInChildren<EnemyAnimations>();
            if (anim != null) Instance._enemyAnimations.Remove(anim);
        }
        #endregion
        
        #region Private Methods
        
        private void ProcessAllFastLogic()
        {
            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                var enemy = _activeEnemies[i];
                if (enemy == null || !enemy.isAlive) continue;

                // --- COMBAT (Runs for everyone so no one ignores the player) ---
                float sqrDistance = (enemy.transform.position - _playerTransform.position).sqrMagnitude;
                float sqrAttackRange = enemy.GetAttackRange() * enemy.GetAttackRange();
                
                if (sqrDistance <= sqrAttackRange)
                {
                    if (Time.time >= enemy.GetNextAttackTime())
                    {
                        enemy.Attack();
                        
                        var anim = enemy.GetComponentInChildren<EnemyAnimations>();
                        if (anim != null) anim.CheckForAttacking();
                        
                        enemy.SetNextAttackTime(Time.time + enemy.GetAttackCooldown());
                        enemy.StopMovement();
                    }
                }
                else
                {
                    enemy.ResumeMovement();
                }

                // --- SEPARATION ---
                if (enemy.data == null) continue;

                int hitCount = Physics.OverlapSphereNonAlloc(
                    enemy.transform.position, 
                    enemy.data.seperationRadius, 
                    _hitColliders, 
                    _enemyLayer
                );

                for (int j = 0; j < hitCount; j++)
                {
                    var other = _hitColliders[j];
                    if (other.gameObject != enemy.gameObject)
                    {
                        Vector3 pushDirection = enemy.transform.position - other.transform.position;
                        if (pushDirection.sqrMagnitude > 0.001f) 
                        {
                            enemy.transform.position += pushDirection.normalized * (enemy.data.seperationAmount * Time.deltaTime);
                        }
                    }
                }
            }
        }

        private void ProcessPathfindingChunk(bool playerMoved)
        {
            int processed = 0;

            while (processed < _pathfindingProcessedPerFrame && _activeEnemies.Count > 0)
            {
                if (_currentPathIndex >= _activeEnemies.Count) _currentPathIndex = 0;

                var enemy = _activeEnemies[_currentPathIndex];

                if (enemy == null || !enemy.isAlive)
                {
                    _activeEnemies.RemoveAt(_currentPathIndex);
                    continue; 
                }

                // Only calculate the heavy NavMesh path for this chunk of enemies
                if (playerMoved || Time.time >= enemy.GetLastUpdateTime() + enemy.GetUpdateInterval())
                {
                    enemy.UpdatePathfinding(_playerTransform.position);
                    enemy.SetLastUpdateTime(Time.time);
                }

                processed++;
                _currentPathIndex++; 
            }
        }

        private void ProcessAnimationChunk()
        {
            int processed = 0;
            
            while (processed < _animationsProcessedPerFrame && _enemyAnimations.Count > 0)
            {
                if (_currentAnimIndex >= _enemyAnimations.Count) _currentAnimIndex = 0;

                var anim = _enemyAnimations[_currentAnimIndex];
                
                if (anim != null && anim.enabled)
                {
                    anim.ProcessAnimation();
                    _currentAnimIndex++;
                }
                else
                {
                    _enemyAnimations.RemoveAt(_currentAnimIndex);
                }
                
                processed++;
            }
        }
        #endregion
    }
}