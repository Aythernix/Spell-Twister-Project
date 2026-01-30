using UnityEngine;
using MiniBonk.Enemy;

public class Water_ProjectileScript : MonoBehaviour
{
    // ------------------- Script that handles Only Water Burst Scripting -----------------------//

    // parameters

    float waterProjectileLife = 4f;
    public WeaponDamageStats weaponDamageStats;
    bool hasHit = false;

    // ------------------------ Projectile Lifetime Timer ---------------------------//

    private void Update()
    {
        waterProjectileLife -= Time.deltaTime; // constantly counts down the bullets life timer as soon as its awake
        if (waterProjectileLife <= 0) Destroy(gameObject); // when the projectiles life is up, kills projectile.
    }

    // -------------------------- Test if collided --------------------------------//

    private void OnTriggerEnter(Collider other)
    {
        // Makes sure that collision code doesn't run twice due to timings
        if (hasHit) return;

        if (other.CompareTag("Enemy")) // Checks if projectile is colliding with an object with tag 'Enemy'
        {
            // sets the Boolean
            hasHit = true; 

            // turns off projectile collider so it can't trigger code more than once
            GetComponent<Collider>().enabled = false;

            // creates a local variable for the Enemy Class
            var justHit = other.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null) // tests if the enemy that was hit still exists
            {
                // adds damage accordingly
                justHit.TakeDamage(weaponDamageStats.waterBallProjectileDMG); 
            }

            // kills off projectile
            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment")) // Checks if projectile is colliding with an object with tag 'Environment'
        {
            // sets the Boolean
            hasHit = true;

            // kills off projectile
            Destroy (gameObject);
        }
    }
}
