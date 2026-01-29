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

    //public TestEnemy enemy;
   // public WeaponManager weaponManager;
    public WeaponDamageStats weaponDamageStats;

    private void Update()
    {
        //FireBallTimer();
        bulletLife -= Time.deltaTime;
        if (bulletLife <= 0) Destroy(gameObject);
    }

    void FireBallTimer()
    {
        // ------ Timer that destroys bullet when its lifetime runs out --------//

        if (bulletLife >= 0)
        {
            bulletLife -= Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (hasHit) return;

        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasHit = true;

            if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;

            if (TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.linearVelocity = Vector3.zero; 
           

            var justHit = collision.gameObject.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null)
            {
                
                justHit.TakeDamage(weaponDamageStats.fireBallProjectileDMG);
            }
            Destroy(gameObject);
        }

        // ------ Destroys bullet when it collides with an object with the 'Environment' tag ------//

        if (collision.gameObject.CompareTag("Environment"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return ;
        
        if(other.CompareTag("Enemy"))
        {
            hasHit= true;

            GetComponent<Collider>().enabled = false;

            var justHit = other.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null)
            {
                justHit.TakeDamage(weaponDamageStats.fireBallProjectileDMG);
            }

            Destroy (gameObject);
        }
        else if (other.gameObject.CompareTag("Environment"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
