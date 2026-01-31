using UnityEngine;
using System;

public class Weapon_FireType : MonoBehaviour
{
    // ------------------------------ CODE FOR FIRE TYPE WEAPON -----------------------//

    // bullet parameters
    public int fireBallsMax = 10;
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

    void createFireBall()
    {
        // resets the timer for when the last bullet was shot
        lastFireBall = 0;

        if (fireBallPrefab == null || projectileSpawner == null) return;

        // Sets 'bullet' spawn at 'bullet spawners' position
        var bullet = Instantiate(fireBallPrefab, projectileSpawner.transform.position, projectileSpawner.transform.transform.rotation, gameObject.transform);

        // creates a new unnasigned vector 3 for the fireBalls move direction
        Vector3 bulletDirection = projectileSpawner.transform.forward;

        // creates a new vector with the 'closestEnemy' variable from the range detector script
        Transform enemyTarget = enemyDetector.CheckForClosestEnemy();

        if (enemyTarget != null)
        {
            try
            {
                bulletDirection = (enemyTarget.position - projectileSpawner.transform.position).normalized;
            }
            catch (MissingReferenceException)
            {
                bulletDirection = projectileSpawner.transform.forward;
            }
        }


        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bulletDirection * fireBallSpeed;
        }


        // removes a bullet each time the function is called
        fireBalls -= 1;

        // assigns the timer to default after being used
        reloadTimer = reloadTime;
    }
}
