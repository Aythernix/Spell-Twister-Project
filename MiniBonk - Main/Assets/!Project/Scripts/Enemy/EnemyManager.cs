using System;
using System.Collections;
using System.Collections.Generic;
using Minibonk.Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MiniBonk.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        #region Variables
        [Header("Enemies")]
        [SerializeField] private List<GameObject> _enemyTypes = new List<GameObject>();
        
        [SerializeField] private List<GameObject> _enemiesInScene  = new List<GameObject>();
        
        [Header("SpawnSettings")]
        // How many seconds between enemy spawns
        [SerializeField] private float _spawnDelay = 5;
        
        [SerializeField] private float _maxDistanceToPlayer = 10;
        // How many enemies to spawn at once
        [SerializeField] private int _enemiesToSpawnPerBatch = 5;
        // The area around the player enemies will spawn
        [SerializeField] private float _spawnRadius;
        // Maximum number of enemies to allow in the scene
        [SerializeField] private int _maxEnemiesInScene;
        // Layer of ground
        [SerializeField] private LayerMask _groundLayer;

        private Transform _playerTransform;
        private bool _validSpawn = true;
        private bool _maxReached;
        private Vector3 _lastSpawnPos = Vector3.zero;
        private float lastManagerUpdateTime;
        private const float MANAGER_UPDATE_INTERVAL = 0.1f; // Check 10 times per second
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            GameManager.Instance.EventHandler.OnEnemyKilled += OnEnemyKilled;
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease += ChaosMultiplier;
        }
        

        private void OnDisable()
        {
            GameManager.Instance.EventHandler.OnEnemyKilled -= OnEnemyKilled;
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease -= ChaosMultiplier;
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            lastManagerUpdateTime = Time.time;
            StartEnemySpawning();
        }

        // Update is called once per frame
        void Update()
        {
            // Only update manager at intervals to reduce CPU load
            if (Time.time >= lastManagerUpdateTime + MANAGER_UPDATE_INTERVAL)
            {
                #region Debugs

                if (!_validSpawn)
                {
                    Debug.LogWarning($"Enemies are being prevented from spawning {_lastSpawnPos}");
                }

                #endregion

                if (_enemiesInScene.Count >= _maxEnemiesInScene)
                {
                    HandleMaxEnemy();
                }
                else
                {
                    _maxReached = false;
                }
                
                lastManagerUpdateTime = Time.time;
            }
        }
        #endregion
        
        #region private methods
        
        private void ChaosMultiplier(int level)
        {
            _spawnDelay = Mathf.Clamp(_spawnDelay - GameManager.Instance.ChaosMultiplier(), 3, _spawnDelay);
            _maxEnemiesInScene += Mathf.RoundToInt(GameManager.Instance.ChaosMultiplier());
        }
        private void StartEnemySpawning()
        {
            if (GameManager.Instance.sceneIsPlayable)
            {
                _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                StartCoroutine(HandleEnemySpawning());
            }
        }
        // Gets a random position in a sphere around the player
        private Vector3 SpawnPosition()
        {
            Vector3 point = Random.insideUnitSphere *  _spawnRadius;
            RaycastHit hit;
            Vector3 spawnPos = _playerTransform.position + point;
            bool hasHit = Physics.Raycast(spawnPos, Vector3.down, out hit, float.PositiveInfinity, _groundLayer);
            
            if (Vector3.Distance(spawnPos, _playerTransform.position) < _maxDistanceToPlayer)
            {
                hasHit = false;
            }
            
            spawnPos.y = hit.point.y;
            _validSpawn = hasHit;
            _lastSpawnPos = spawnPos;
            
            return spawnPos;
        }

        // Handles the enemy spawn rate (Seconds)
        private IEnumerator HandleEnemySpawning()
        {
            yield return new WaitForSeconds(_spawnDelay - GameManager.Instance.ChaosMultiplier());
            


            if (!_maxReached)
            {
                SpawnEnemy();
            }
            
            if (!GameManager.Instance.sceneIsPlayable)
                yield break;
            
            StartCoroutine(HandleEnemySpawning());
        }
        
        /// <summary>
        /// Spawns enemies using weighted random selection based on each enemy's spawn chance.
        /// Each enemy type has a probability of being selected proportional to its spawnChance value.
        /// </summary>
        private void SpawnEnemy()
        {
            Debug.Log($"Spawning {_enemiesToSpawnPerBatch * GameManager.Instance.ChaosMultiplier()} enemies");
            // Spawn multiple enemies per batch as configured
            int totalEnemies = (int)(_enemiesToSpawnPerBatch * GameManager.Instance.ChaosMultiplier());
            int maxAttemptsPerEnemy = 10; // Failsafe to prevent crash

            for (int i = 0; i < totalEnemies; i++)
            {
                bool spawnSuccessful = false;
    
                // Attempt to find a position up to maxAttemptsPerEnemy times
                for (int attempt = 0; attempt < maxAttemptsPerEnemy; attempt++)
                {
                    var spawnPos = SpawnPosition();
                    if (_validSpawn && !_maxReached)
                    {
                        var selectedEnemy = GetWeightedRandomEnemy();
                        if (selectedEnemy != null)
                        {
                            var enemyAdded = Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
                            _enemiesInScene.Add(enemyAdded);
                            spawnSuccessful = true;
                        }
                        break; // Exit the attempt loop once successfully spawned
                    }
                }

                if (!spawnSuccessful)
                {
                    Debug.LogWarning($"Could not find valid spawn for enemy {i} after {maxAttemptsPerEnemy} tries.");
                }
            }
        }

        /// <summary>
        /// Selects an enemy type using weighted random selection based on spawnChance values.
        /// Enemies with higher spawnChance have higher probability of being selected.
        /// </summary>
        /// <returns>The selected enemy prefab, or null if no valid enemies found</returns>
        private GameObject GetWeightedRandomEnemy()
        {
            // Calculate the total spawn chance across all enemy types
            float totalChance = 0f;
            foreach (var enemy in _enemyTypes)
            {
                // Get the Enemy component to access the EnemyData
                var enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.data != null)
                {
                    // Accumulate the spawn chance for this enemy type
                    totalChance += enemyComponent.data.spawnChance;
                }
            }

            // If no spawn chances are set (all zero or no valid data), fall back to random selection
            if (totalChance <= 0f)
            {
                Debug.LogWarning("No valid spawn chances found, using random selection");
                return _enemyTypes[Random.Range(0, _enemyTypes.Count)];
            }
            
            float currentChance = 0f;

            // Iterate through enemies to find which one the random value falls into
            foreach (var enemy in _enemyTypes)
            {
                var enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null && enemyComponent.data != null)
                {
                    // Add this enemy's spawn chance to the running total
                    currentChance += enemyComponent.data.spawnChance;
                    
                    // If the random value falls within this enemy's range, select it
                    if (GameManager.Instance.Probability(currentChance, totalChance))
                    {
                        return enemy;
                    }
                }
            }

            // Safety fallback - should never reach here if totalChance > 0
            Debug.LogWarning("Weighted selection failed, falling back to first enemy");
            return _enemyTypes[0];
        }

        private void HandleMaxEnemy()
        {
            _maxReached = true;
            
            // // Only runs following code if the enemies in the scene exceed the max
            // if (_enemiesInScene.Count <= _maxEnemiesInScene) return;
            //
            // // Returns the enemy at the last index of the list
            // var nextEnemy = _enemiesInScene[^1];
            //
            // if (nextEnemy != null)
            // {
            //     Destroy(nextEnemy);
            // }
            //
            // HandleRemoveEnemy(nextEnemy);
        }
        
        // Passing parameters from Enemy Killed event to the handle enemy removed
        private void OnEnemyKilled(GameObject killer, GameObject enemyToRemove)
        {
            HandleRemoveEnemy(enemyToRemove);
        }
        
        private void HandleRemoveEnemy(GameObject enemyToRemove)
        {
            _enemiesInScene.Remove(enemyToRemove);
        }

        #endregion
    }
}
