using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
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
                    var bullet = Instantiate(bulletPrefab, bulletSpawner.transform);

                    // calculates bullets shooting
                    bullet.GetComponent<Rigidbody>().linearVelocity = (bulletSpawner.transform.forward * bulletSpeed);

                    // stops the bullets rotation locking with 'bulletSpawner' after being shot
                    bullet.transform.SetParent(null);

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
