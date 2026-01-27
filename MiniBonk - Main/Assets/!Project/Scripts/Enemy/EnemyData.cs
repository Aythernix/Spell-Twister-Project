using UnityEngine;

namespace MiniBonk.Enemy
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Movement Settings")] 
        public float speed = 3.5f;
        public float acceleration = 8f;
        public float stoppingDistance = 5f;

        [Header("Combat Settings")] public int health = 100;
        public float damage = 10f;
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        public int xpValue = 50;

        [Header("Visuals")] public GameObject model;
        public Color debugColor = Color.red;

    }
}