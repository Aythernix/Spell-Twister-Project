using UnityEngine;
using MiniBonk.Enemy;

public class Ice_ProjectileScript : MonoBehaviour
{
    // -------------------- Script that handles only Ice projectile ----------------------//

    float bulletLife = 10f;
    private bool hasHit = false;
    public WeaponDamageStats weaponDamageStats;

    public GameObject iceDOTZone;

    // ------------------------ Main Logic -----------------------// 

    private void Update()
    {
        bulletLife -= Time.deltaTime;
        if (bulletLife <= 0f) Destroy(gameObject);
    }

    // ---------------------------- Test If collided -----------------------//

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            // --------------- Add weapon Damage to Enemy Hit ---------------------//

            //hasHit = true;
            GetComponent<Collider>().enabled = false;

            var justHit = other.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null)
            {
                var frostBiteZone = Instantiate(iceDOTZone, justHit.transform.position, Quaternion.identity);
                justHit.TakeDamage(weaponDamageStats.iceBallProjectileDMG, transform.root.gameObject);
                
                //var frostBiteZone = Instantiate(iceDOTZone, justHit.transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
            // ----------------- Adds FrostBite DOT Effect ---------------//



        }

        if (other.CompareTag("Environment"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
