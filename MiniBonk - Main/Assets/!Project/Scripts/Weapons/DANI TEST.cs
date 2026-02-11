using UnityEngine;

public class DANITEST : MonoBehaviour
{
    // ---------- Parameters --------//

    public GameObject BulletPrefab;
    public GameObject BulletSpawner;

    int bulletSpeed = 20;
    float lastBullet = 0;
    float fireRate = 0.5f;

    Rigidbody rb;


    // -------------------- Methods ---------------//

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && (lastBullet > fireRate))
        {
            createBullet();
        }
        else
            lastBullet += Time.deltaTime;
    }

    void createBullet()
    {
        var bullet = Instantiate(BulletPrefab,BulletSpawner.transform.position,Quaternion.identity);
        Vector3 bulletDirection = (BulletSpawner.transform.forward);
        rb = bullet.GetComponent<Rigidbody>();

        rb.linearVelocity = bulletDirection * bulletSpeed;

        lastBullet = 0;
    }
}
