using Minibonk.Management;
using UnityEngine;
using Minibonk.Player;
namespace MiniBonk.Enemy
{
    public class RangedEnemy : Enemy
    {
        [Header("Ranged Setup")] [SerializeField]
        private Transform _firePoint;
        
        

        public override void Attack()
        {
            if (data.projectilePrefab == null)
            {
                Debug.LogWarning("Projectile prefab is not assigned in EnemyData.");
                return;
            }
            
            // Aim at the player's center (adjust height as needed)
            Vector3 targetCenter = player.position + Vector3.up * 1f;
            // Calculate the direction from the fire point to the target center
            Vector3 fireDirection = (targetCenter - _firePoint.position).normalized;
            
          
            //Rotates enemy's body to face player on Y axis
            Vector3 bodyLookDir = fireDirection;
            bodyLookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(bodyLookDir);
            //Insantiates the projectile 
            GameObject projectileGO = Instantiate(data.projectilePrefab, _firePoint.position, Quaternion.LookRotation(fireDirection));
            Debug.Log("Firing projectile towards " + targetCenter);
            
             
            if (projectileGO.TryGetComponent<Projectile>(out var projectile))
            {
                // Pulls data from scriptable object
                projectile.Setup(data.damage, data.projectileSpeed, data.projectileLifetime);
            }
            GameManager.Instance.EventHandler.PlayAudio(AudioManager.AudioClips.RangedAttacks, _enemyAudioSource);
        }
    }
}