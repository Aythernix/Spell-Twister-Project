using System;
using UnityEngine;
using UnityEngine.AI;

namespace MiniBonk.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header ("Settings")]
        public EnemyData data; // Scriptable Object containing the enemy settings
        public Transform player;
        private NavMeshAgent navMeshAgent;
        

        // Update is called once per frame
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void ApplySettings()
        {
            navMeshAgent.speed = data.speed;
            navMeshAgent.acceleration = data.acceleration;
            navMeshAgent.stoppingDistance = data.stoppingDistance;
        }
        void FixedUpdate()
        {

            navMeshAgent.SetDestination(player.position);
        }
    }
}
