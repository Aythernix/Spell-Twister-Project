using Minibonk.Management;
using MiniBonk.Enemy;
using UnityEngine;

public class Ice_ProjectileScript : MonoBehaviour
{
    // -------------------- Script that handles only Ice projectile ----------------------//

    float bulletLife = 10f;
    private bool hasHit = false;
    public WeaponDamageStats weaponDamageStats;
    private Collider projectileCollider; // Cached collider

    public GameObject iceDOTZone;

    private void Awake()
    {
        projectileCollider = GetComponent<Collider>();
    }

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
            projectileCollider.enabled = false;

            var justHit = other.GetComponent<MiniBonk.Enemy.Enemy>();

            if (justHit != null)
            {
                var frostBiteZone = Instantiate(iceDOTZone, justHit.transform.position, Quaternion.identity);

                // Call Sound effect
                GameManager.Instance.EventHandler.PlayAudio?.Invoke(AudioManager.AudioClips.IceImpact, GameManager.Instance.currentLevelManager.playerHandler.playerAudioSource);

                justHit.TakeDamage(weaponDamageStats.iceBallProjectileDMG, transform.root.gameObject);
                
                //var frostBiteZone = Instantiate(iceDOTZone, justHit.transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Environment"))
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
