using Unity.VisualScripting;
using UnityEngine;
using MiniBonk.Enemy;
using System.Runtime.CompilerServices;
using Minibonk.Management;

public class Fire_ProjectileScript : MonoBehaviour
{
    // ------------- Script that handles only Fireball Projectile spawning ---------------//

    // Constant homing parameters
    [HideInInspector] public Transform target;
    public float speed = 30f;
    public float turnSpeed = 500f;

    // Bullet parameters
    [Header("Bullet")]
    float bulletLife = 2.5f;
    private bool hasHit = false;
    public WeaponDamageStats weaponDamageStats;

    private Collider projectileCollider; // Cached collider
    private Rigidbody rb;

    private void Awake()
    {
        projectileCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (hasHit) return;

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
            } 
        }

        rb.linearVelocity = transform.forward * speed;
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
                justHit.TakeDamage(weaponDamageStats.fireBallProjectileDMG, GameManager.Instance.currentLevelManager.playerHandler.gameObject);
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
