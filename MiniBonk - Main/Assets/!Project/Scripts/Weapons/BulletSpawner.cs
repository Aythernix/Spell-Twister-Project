using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class BulletSpawner : MonoBehaviour
{
    // bullet parameters
    public int bulletsMax = 10;
    public int bullets = 30;
    public float fireRate;
    public int bulletSpeed = 30;
    float lastBullet;

    public float reloadTimer = 2f;
    public float reloadTime = 1f;

    // Get bullet prefabs
    public GameObject bulletPrefab;
    public GameObject bulletSpawner;

    // Get the enemy Range detector script
    public EnemyRangeDetector enemyDetector;

    private void Awake()
    {
        // assigns variables to match on awake
        bullets = bulletsMax;
        reloadTimer = reloadTime;
    }
    void Update()
    {
       shootBullet();      
    }

    void shootBullet()
    {
        if (bullets != 0)
        {
            // reactivates the function after a certain amout of time (fire rate)
            if (Time.deltaTime - lastBullet < fireRate)
            {
                if (Input.GetMouseButtonDown(0) && (bullets >= 0))
                {
                    // Sets 'bullet' spawn at 'bullet spawners' position
                    var bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.transform.transform.rotation);

                    // creates a new vector with the 'closestEnemy' variable from the range detector script
                    Transform enemyTarget = enemyDetector.CheckForClosestEnemy();

                    // creates a new unnasigned vector 3 for the bullets move direction
                    Vector3 bulletDirection;

                    // if statement that checks if an enemy is in the hitbox of the range detector
                    if (enemyTarget != null)
                    {
                        // sets the next bullets direction toward the closest enemies position
                        bulletDirection = (enemyTarget.position - bulletSpawner.transform.position).normalized;
                    }
                    else
                    {
                        // sets the next bullets direction as the players forward transform (forward)
                        bulletDirection = bulletSpawner.transform.forward;
                    }

                    // calculates bullets shooting
                    bullet.GetComponent<Rigidbody>().linearVelocity = bulletDirection * bulletSpeed;

                    // removes a bullet each time the function is called
                    bullets -= 1;

                    // assigns the timer to default after being used
                    reloadTimer = reloadTime;
                }
                return;
            }
            lastBullet = Time.deltaTime;
            
        }
        
        // runs when bullets hit or go below 0
        if (bullets <= 0)
        {
           // checks if the timer is greater than 0
           if (reloadTimer > 0)
           {
                // assigns timer based on deltatime
                reloadTimer -= Time.deltaTime;
           }
           else
           {
                // resets the values after the timer hits 0
                bullets = bulletsMax;
                reloadTimer = 0;
           }
        }  
    }
}
