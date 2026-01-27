using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniBonk.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        #region Variables
        [Header("Enemies")]
        [SerializeField] private List<GameObject> _enemyTypes = new List<GameObject>();
        
        private Stack<GameObject> _enemiesInSceneStack  = new Stack<GameObject>();
        
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
        
        private bool canSpawn = true;
        private Vector3 lastSpawnPos = Vector3.zero;
        
        #endregion
        
        #region Unity Methods
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

            if (!canSpawn)
            {
                Debug.LogWarning($"Enemies are being prevented from spawning {lastSpawnPos}");
            }

            #endregion
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
            canSpawn = hasHit;
            lastSpawnPos = spawnPos;
            
            return spawnPos;
        }

        // Handles the enemy spawn rate (Seconds)
        private IEnumerator HandleEnemySpawning()
        {
            
            yield return new WaitForSeconds(_spawnRate);
            
            SpawnEnemy();
        }
        
        // Spawns an enemy of a random type
        private void SpawnEnemy()
        {
            for (int i = 0; i < _enemiesToSpawnPerBatch; i++)
            {
                var spawnPos = SpawnPosition();
                if (!canSpawn) continue;
                var enemyTypeInex = Random.Range(0, _enemyTypes.Count - 1);
                var enemyAdded = Instantiate(_enemyTypes[enemyTypeInex],  spawnPos, Quaternion.identity);
                _enemiesInSceneStack.Push(enemyAdded);
            }
            StartCoroutine(HandleEnemySpawning());
        }
        #endregion
    }
}
