using Unity.VisualScripting;
using UnityEngine;
using MiniBonk.Enemy;
using System.Runtime.CompilerServices;

public class Fire_ProjectileScript : MonoBehaviour
{
    // ------------- Script that handles only Fireball Projectile spawning ---------------//
     
    // Bullet parameters

    float bulletLife = 2.5f;
    private bool hasHit = false;
    public WeaponDamageStats weaponDamageStats;
    private Collider projectileCollider; // Cached collider

    private void Awake()
    {
        projectileCollider = GetComponent<Collider>();
    }

    // ------------------------ Projectile Lifetime Timer ---------------------------//
    private void Update()
    {
        bulletLife -= Time.deltaTime; // constantly counts down the bullets life timer as soon as its awake
        if (bulletLife <= 0) Destroy(gameObject); // when the projectiles life is up, kills projectile.
    }

    // -------------------------- Test if collided --------------------------------//

    private void OnTriggerEnter(Collider other)
    {
        // Makes sure that collision code doesn't run twice due to timings
        if (hasHit) return ;
        
        if (other.CompareTag("Enemy")) // Checks if projectile is colliding with an object with tag 'Enemy'
        {
            // sets the Boolean
            hasHit = true;

            // turns off projectile collider so it can't trigger code more than once
            projectileCollider.enabled = false;

            // creates a local variable for the Enemy Class
            var justHit = other.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null) // tests if the enemy that was hit still exists
            {
                // adds damage accordingly
                justHit.TakeDamage(weaponDamageStats.fireBallProjectileDMG, transform.root.gameObject);
            }

            // kills off projectile
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Environment")) // Checks if projectile is colliding with an object with tag 'Environment'
        {
            // sets the Boolean
            hasHit = true;

            // kills off projectile
            Destroy(gameObject);
        }
    }
}
