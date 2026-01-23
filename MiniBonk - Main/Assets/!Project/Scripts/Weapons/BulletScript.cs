using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Bullet parameters

    float bulletLife = 2.5f;

    private void Update()
    {
        bulletTimer();
    }

    void bulletTimer()
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
    }
}
