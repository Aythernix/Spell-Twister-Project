using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Bullet parameters

    float bulletLife = 2.5f;

    private void Update()
    {
        if (bulletLife >= 0)
        {
            bulletLife -= Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }



}
