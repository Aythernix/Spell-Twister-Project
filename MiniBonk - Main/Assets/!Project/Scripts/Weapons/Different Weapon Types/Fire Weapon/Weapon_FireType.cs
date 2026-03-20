using UnityEngine;
using System;
using Minibonk.Management;

public class Weapon_FireType : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease += ApplyChaosMultiplier;
    }
    private void OnDisable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease -= ApplyChaosMultiplier;
    }
    // ------------------------------ CODE FOR FIRE TYPE WEAPON -----------------------//

    // bullet parameters
    public int fireBallsMax = 15;
    public int fireBalls = 30;
    public float fireRate = 0.5f;
    public int fireBallSpeed = 30;
    float lastFireBall;

    public float reloadTimer = 2f;
    public float reloadTime = 2f;

    // --------------------- Options for weapon to become manual or automatic ---------------//

    public bool autoFire = true;

    // ------------------------------- Get bullet prefabs -----------------------------//
    public GameObject fireBallPrefab;
    public GameObject projectileSpawner;

    // Get the enemy Range detector script
    public EnemyRangeDetector enemyDetector;

    // ------------------------------ Timer for sounds ------------------------------------//

    private float _soundTimer = 10f;
    public float timeToWaitForSound;

    private void Awake()
    {
        // assigns variables to match on awake
        fireBalls = fireBallsMax;
        reloadTimer = reloadTime;
    }

    public void shootFireBall()
    {
        // -------------------------- RELOADING LOGIC -----------------------//

        // runs when fireBalls hit or go below 0
        if (fireBalls <= 0)
        {
            // assigns timer based on deltatime
            reloadTimer -= Time.deltaTime;

            // checks if the timer is greater than 0
            if (reloadTimer <= 0)
            {
                // resets the values after the timer hits 0
                fireBalls = fireBallsMax;
                reloadTimer = reloadTime;

            }
            return;
        }

        // ------------------------- FIRE RATE TIMER LOGIC ---------------------------------//

        lastFireBall += Time.deltaTime;
        _soundTimer += Time.deltaTime;

        // ------------------------- SPAWNING FIREBALL LOGIC -------------------------------//

        // This code only runs when the 'autoFire' function is turned on
        if (autoFire)
        {
            if (lastFireBall >= fireRate)
            {
                // spawns fireBall
                 createFireBall();
            }
        }
        else // This else statement is called when the shooting is in 'Manual' mode
        {
            // calls the if statement if the firerate timer has passed and left click is pressed
            if (Input.GetMouseButtonDown(0) && lastFireBall >= fireRate)
            {
                // spawns fireBall
                createFireBall();
            }
        }
    }
    
    private void ApplyChaosMultiplier(int lvl)
    {
            fireRate = Mathf.Clamp(fireRate / GameManager.Instance.ChaosMultiplier(), 0.1f, fireRate);   
    }

    void createFireBall()
    {

        #region 

        // Timer for sounds to be called
        

        if (_soundTimer > timeToWaitForSound)
        {
            // Call Sound effect
            GameManager.Instance.EventHandler.PlayAudio?.Invoke(AudioManager.AudioClips.Fireball, GameManager.Instance.currentLevelManager.playerHandler.playerAudioSource);
            _soundTimer = 0;
        }
    
        #endregion

        // Allows for weapon to communicate to GameManager
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnSpellFired?.Invoke(SpellTypes.FireballSpell);

        // resets the timer for when the last bullet was shot
        lastFireBall = 0;

        if (fireBallPrefab == null || projectileSpawner == null) return;

        // Sets 'bullet' spawn at 'bullet spawners' position
        var bullet = Instantiate(fireBallPrefab, projectileSpawner.transform.position, projectileSpawner.transform.transform.rotation);
        bullet.transform.parent = null;

        // Grabs the script on new fireball to allow homing
        Fire_ProjectileScript script = bullet.GetComponent<Fire_ProjectileScript>();

        if (script != null)
        {
            script.target = enemyDetector.CheckForClosestEnemy();
            script.speed = fireBallSpeed;
        }

        // removes a bullet each time the function is called
        fireBalls -= 1;

        // assigns the timer to default after being used
        reloadTimer = reloadTime;
    }
}
