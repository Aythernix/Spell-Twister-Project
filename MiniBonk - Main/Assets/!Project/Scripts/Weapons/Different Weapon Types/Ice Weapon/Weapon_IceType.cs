using Minibonk.Management;
using UnityEngine;

public class Weapon_IceType : MonoBehaviour
{
    // ------------------- Code for Ice weapon ------------------------//

    // parameters

    public GameObject iceBallPrefab;
    public GameObject projectileSpawner;
    public EnemyRangeDetector enemyRangeDetector; // the range will be 30M
    public bool autoFire = true;

    public int iceProjectile = 0;
    public int iceProjectileMax = 5;
    public int iceProjectileSpeed = 10;

    public float fireRate = 2f;
    public float reloadTime = 4f;
    public float reloadTimer = 0;

    public float lastProjectile = 0;

    // ------------------------- Core Logic ---------------------------//
    private void Start()
    {
        iceProjectile = iceProjectileMax;
        reloadTimer = reloadTime; // allows switching to have a short cooldown
    }

    public void ShootIceProjectile()
    {
        // ---------------------- Call reload Function ---------------------//

        if (iceProjectile <= 0)
        {
            reloadTimer -= Time.deltaTime;
            
            if (reloadTimer <= 0)
            {
                reloadTimer = reloadTime;
                iceProjectile = iceProjectileMax;
            }
            return;
        }

        // --------------------------- last Projectile Timer -------------------//

        lastProjectile += Time.deltaTime;

        // -------------------------- Spawning a Projectile Logic -----------------------//

        // This code only runs when the 'autoFire' function is turned on
        if (autoFire)
        {
            if (lastProjectile >= fireRate)
            {
                // spawns ice Ball
                CreateIceProjectile();
            }
        }
        else // This else statement is called when the shooting is in 'Manual' mode
        {
            // calls the if statement if the firerate timer has passed and left click is pressed
            if (Input.GetMouseButtonDown(0) && lastProjectile >= fireRate)
            {
                // spawns ice Ball
                CreateIceProjectile();
            }
        }
    }

    private void CreateIceProjectile()
    {
        // Call Sound effect

        GameManager.Instance.EventHandler.PlayAudio?.Invoke(AudioManager.AudioClips.Ice, GameManager.Instance.currentLevelManager.playerHandler.playerAudioSource);

        lastProjectile = 0;

        if (iceBallPrefab == null || projectileSpawner == null) return;

        // Allows for weapon to communicate to GameManager

        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnSpellFired?.Invoke(SpellTypes.IceSpell);

        var newProjectile = Instantiate(iceBallPrefab, projectileSpawner.transform.position, projectileSpawner.transform.rotation, gameObject.transform);
        Vector3 newProjectileDirection = projectileSpawner.transform.forward;

        Transform enemyTarget = enemyRangeDetector.CheckForClosestEnemy();

        if (enemyTarget != null)
        {
            try
            {
                newProjectileDirection = (enemyTarget.position - projectileSpawner.transform.position).normalized;
            }
            catch (MissingReferenceException)
            {
                newProjectileDirection = projectileSpawner.transform.forward;
            }
        }

        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = newProjectileDirection * iceProjectileSpeed;
        }

        iceProjectile -= 1;
        reloadTimer = reloadTime;

    }

}
