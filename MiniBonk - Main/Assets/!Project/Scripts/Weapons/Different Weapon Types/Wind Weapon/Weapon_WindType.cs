using Minibonk.Management;
using Minibonk.Player;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon_WindType : MonoBehaviour
{
    // ------------------- Code for wind Weapon ---------------------//

    // parameters

    public GameObject windProjectilePrefab;
    public GameObject projectileSpawner;
    public EnemyRangeDetector enemyAreaDetector;
    public bool autoFire = true;

    public int windProjectileBurst = 0;
    public int windProjectileBurstMax = 3;
    public int windProjectileSpeed = 40;

    public float windProjectileBurstTime = 0.5f; // Time between each projectile burst
    public float reloadTime = 1.5f;
    public float reloadTimer = 1f;

    public float lastProjectile = 0;

    // ----------- Sound ------------//

    private float _soundTimer = 10f;
    public float timeToWaitForSound;

    // -------------------- Core Logic ---------------------------//

    private void Start()
    {
        windProjectileBurst = windProjectileBurstMax; // sets the default when game starts
        reloadTimer = reloadTime; // Sets the time to max 
    }

    public void ShootWindBlade()
    {
        // ---------------------- Call reload Function ---------------------//

        // if statement that runs when blades reach 0
        if (windProjectileBurst <= 0)
        {
            reloadTimer -= Time.deltaTime; // counts down the reload timer

            if (reloadTimer <= 0) // if statement that checks when the timer reaches 0
            {
                // resets the parameters for the max
                windProjectileBurst = windProjectileBurstMax;
                reloadTimer = reloadTime;
            }
            return; // returns the code so that the code below wont run when its false
        }

        // -------------------------- Last Projectile Timer ------------------------//

        lastProjectile += Time.deltaTime; // constantly sets the last projectile timer to count up
        _soundTimer += Time.deltaTime;

        // -------------------- Manual and Automatic firing Logic ----------------------//

        if (autoFire) // Activated when autoFire is active
        {
            if (lastProjectile >= windProjectileBurstTime)
            {
                CreateWindProjectile();
            }
        }
        else // Activates when manual mode is active
        {
            if (Input.GetMouseButtonDown(0) && (lastProjectile >= windProjectileBurstTime))
            {
                CreateWindProjectile();
            }
        }
    }

    void CreateWindProjectile()
    {
        #region 

        // Timer for sounds to be called

        if (_soundTimer > timeToWaitForSound)
        {
            // Call Sound effect
            GameManager.Instance.EventHandler.PlayAudio?.Invoke(AudioManager.AudioClips.Wind, GameManager.Instance.currentLevelManager.playerHandler.playerAudioSource);
            _soundTimer = 0;
        }

        #endregion

        lastProjectile = 0; // Resets last projectile timer to 0
        if (windProjectilePrefab == null || projectileSpawner == null) return; // safety line that makes sure the prefab and spawner actually exist

        // Allows for weapon to communicate to GameManager

        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnSpellFired?.Invoke(SpellTypes.WindSpell);

        // creates and makes a set vector 3 to be used on the newely made projectile
        Vector3 windProjectileDirection = projectileSpawner.transform.forward;
        Transform enemyTarget = enemyAreaDetector.CheckForClosestEnemy();

        if (enemyTarget != null)
        {
            try
            {
                windProjectileDirection = (enemyTarget.position - projectileSpawner.transform.position).normalized;
            }
            catch (MissingReferenceException)
            {
                windProjectileDirection = projectileSpawner.transform.forward;
            }
        }

        Quaternion spawnRotation = Quaternion.LookRotation(windProjectileDirection);

        var newWindProjectile = Instantiate(windProjectilePrefab, projectileSpawner.transform.position, spawnRotation);
        
        newWindProjectile.transform.parent = null;
        

        // sets the new projectile to a rigid body
        Rigidbody rb = newWindProjectile.GetComponent<Rigidbody>();
        if (rb != null) // runs if statement if a rigidbody exists
        {
            rb.linearVelocity = windProjectileDirection * windProjectileSpeed; // adds to the rigidbodys velocity
        }

        // takes away a wind burst after creating a projectile
        windProjectileBurst -= 1;
        
    }
}
