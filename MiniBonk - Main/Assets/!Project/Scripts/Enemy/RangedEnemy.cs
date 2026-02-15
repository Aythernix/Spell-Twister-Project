using UnityEngine;
using Minibonk.Player;
namespace MiniBonk.Enemy
{
    public class RangedEnemy : Enemy
    {
        [Header("Ranged Setup")] [SerializeField]
        private Transform _firePoint;
        

        protected override void Attack()
        {
            if (data.projectilePrefab == null)
            {
                Debug.LogWarning("Projectile prefab is not assigned in EnemyData.");
                return;
            }
            
            Vector3 targetCenter = player.position + Vector3.up * 1f; // Aim at the player's center (adjust height as needed)
            Vector3 fireDirection = (targetCenter - _firePoint.position).normalized;
            
            // Draw a green line in the Scene view for 1 second showing the fire path
            Debug.DrawRay(_firePoint.position, fireDirection * 10f, Color.green, 1f);
            
            Vector3 bodyLookDir = fireDirection;
            bodyLookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(bodyLookDir);
            
            GameObject projectileGO = Instantiate(data.projectilePrefab, _firePoint.position, Quaternion.LookRotation(fireDirection));
            if (projectileGO.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.Setup(data.damage, data.projectileSpeed, data.projectileLifetime);
            }
        }
    }
}