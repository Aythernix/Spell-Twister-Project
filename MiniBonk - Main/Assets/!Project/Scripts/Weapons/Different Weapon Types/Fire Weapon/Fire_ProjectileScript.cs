using Unity.VisualScripting;
using UnityEngine;

public class Fire_ProjectileScript : MonoBehaviour
{
    // ------------- Script that handles only Fireball Projectile spawning ---------------//

    // Bullet parameters

    float bulletLife = 2.5f;

    private void Update()
    {
        FireBallTimer();
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
        // ------ Destroys bullet when it collides with an object with the 'Environment' tag ------//

        if (collision.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {

            Destroy(gameObject);
        }
    }
}
