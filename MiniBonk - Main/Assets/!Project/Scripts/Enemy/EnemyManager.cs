using System;
using System.Collections;
using System.Collections.Generic;
using Minibonk.Management;
using UnityEngine;
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
        [SerializeField] private float _spawnRate = 5;
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
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            GameManager.Instance.EventHandler.OnEnemyKilled += OnEnemyKilled;
        }

        private void OnDisable()
        {
            GameManager.Instance.EventHandler.OnEnemyKilled -= OnEnemyKilled;
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            StartCoroutine(HandleEnemySpawning());
        }

        // Update is called once per frame
        void Update()
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
        }
        #endregion
        
        #region private methods

        // Gets a random position in a sphere around the player
        private Vector3 SpawnPosition()
        {
            Vector3 point = Random.insideUnitSphere *  _spawnRadius;
            RaycastHit hit;
            Vector3 spawnPos = _playerTransform.position + point;
            bool hasHit = Physics.Raycast(spawnPos, Vector3.down, out hit, float.PositiveInfinity, _groundLayer);
            spawnPos.y = hit.point.y;
            _validSpawn = hasHit;
            _lastSpawnPos = spawnPos;
            
            return spawnPos;
        }

        // Handles the enemy spawn rate (Seconds)
        private IEnumerator HandleEnemySpawning()
        {
            
            yield return new WaitForSeconds(_spawnRate);


            if (!_maxReached)
            {
                SpawnEnemy();
            }
            
            StartCoroutine(HandleEnemySpawning());
        }
        
        // Spawns an enemy of a random type
        private void SpawnEnemy()
        {
            for (int i = 0; i < _enemiesToSpawnPerBatch; i++)
            {
                var spawnPos = SpawnPosition();
                if (!_validSpawn) continue;
                var enemyTypeInex = Random.Range(0, _enemyTypes.Count - 1);
                var enemyAdded = Instantiate(_enemyTypes[enemyTypeInex],  spawnPos, Quaternion.identity);
                _enemiesInScene.Add(enemyAdded);
            }
        }

        private void HandleMaxEnemy()
        {
            _maxReached = true;
            
            // Only runs following code if the enemies in the scene exceed the max
            if (_enemiesInScene.Count <= _maxEnemiesInScene) return;
            
            // Returns the enemy at the last index of the list
            var nextEnemy = _enemiesInScene[^1];
            
            if (nextEnemy != null)
            {
                Destroy(nextEnemy);
            }
            
            HandleRemoveEnemy(nextEnemy);
        }
        
        private void OnEnemyKilled(GameObject killer, GameObject enemyToRemove)
        {
            HandleRemoveEnemy(enemyToRemove, killer);
        }
        
        private void HandleRemoveEnemy(GameObject enemyToRemove, GameObject killer = null)
        {
            _enemiesInScene.Remove(enemyToRemove);
        }

        #endregion
    }
}
