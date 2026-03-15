using UnityEngine;
using MiniBonk.Enemy;
using UnityEngine.Rendering;

public class Wind_ProjectileScript : MonoBehaviour
{
    // ---------------- Script that handles only the Wind Projectile ----------------//

    float bladeLife = 2f;
    private bool hasHit = false;
    public WeaponDamageStats weaponDamageStats;
    private Collider projectileCollider; // Cached collider

    private void Awake()
    {
        projectileCollider = GetComponent<Collider>();
    }

    // --------------------------- Main Logic -----------------------------//

    private void Update()
    {
        bladeLife -= Time.deltaTime;
        if (bladeLife <= 0) Destroy(gameObject);
    }

    // -------------------- Test if collided --------------------------//

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            projectileCollider.enabled = false;

            var justHit = other.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null)
            {
                justHit.TakeDamage(weaponDamageStats.windBallProjectileDMG, transform.root.gameObject);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment"))
        {
            hasHit = true;
            Destroy(gameObject);
        }  
    }
}
