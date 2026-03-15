using UnityEngine;

namespace MiniBonk.Enemy
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Ranged Settings")]
        public GameObject projectilePrefab;
        public float projectileLifetime = 5f;
        public float projectileSpeed = 20f;
        
        [Header("Movement Settings")] 
        public float speed = 3.5f;

        public float speedVariance = 0.5f;
        public float acceleration = 8f;
        public float stoppingDistance = 5f;

        [Header("Combat Settings")] public int health = 100;
        public float damage = 10f;
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public int xpValue = 50;

        [Header("Drops")]
        public GameObject healthDrop;
        public float chanceOfDroppingHealth = 25;

        [Header("Spawn Settings")]
        [Range(0,100)]public float spawnChance;
        
        [Header("Advancced Settings")]
        public float predictionTime = 0.5f; // Time in seconds to predict player's future position for aiming
        public float seperationAmount = 1.5f; // How much enemey avoids others
        public float seperationRadius = 2f; // Distance to keep
        

        [Header("Visuals")] public GameObject model;
        public Color debugColor = Color.red;

    }
}