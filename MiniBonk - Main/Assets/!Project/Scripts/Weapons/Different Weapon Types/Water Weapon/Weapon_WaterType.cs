using Minibonk.Management;
using Unity.Mathematics;
using UnityEngine;

public class Weapon_WaterType : MonoBehaviour
{
    // -------------------- Code for Water Weapon ---------------------//

    // ----- parameters ----- //
    #region 

    // ------------ Main pararmeters ----------------//

    public int waterPulsesMax = 5;
    public int waterPulses;
    public int waterBalls = 20;
    public float waterPulseFireRate = 2f;
    public int waterBallSpeed = 15;

    public float lastwaterPulse;

    public float reloadTime = 3.5f;
    public float reloadTimer;

    // ---------------- special Parameters -----------//

    public GameObject waterBallProjectile;
    public GameObject waterBallSpawner; // This should be the player
    public bool autofire = true;

    float waterballDiv;

    #endregion 

    // -------------------- Main logic -------------------//

    private void Awake()
    {
        waterPulses = waterPulsesMax;
        reloadTimer = reloadTime;

        waterballDiv = 360/ waterBalls;
    }

    public void ShootWaterPulse()
    {
        // ------------------ call reload function -------------------//

        if (waterPulses <= 0)
        {
            reloadTimer -= Time.deltaTime;


            if (reloadTimer <= 0)
            {
                waterPulses = waterPulsesMax;
                reloadTimer = reloadTime;
            }
            return;
        }

        // ------------------ Fire Rate Logic ---------------------//

        lastwaterPulse += Time.deltaTime;

        // ------------------ Spawning the water Pulse -----------------------//

        if (autofire)
        {
            if (lastwaterPulse >= waterPulseFireRate)
            {
                CreateWaterPulse();
            }
        }
        else // Activates while in manual mode
        {
            if (lastwaterPulse >= waterPulseFireRate && Input.GetMouseButtonDown(0))
            {
                CreateWaterPulse();
            }
        }
    }

    private void CreateWaterPulse()
    {
        lastwaterPulse = 0;

        if (waterBallProjectile == null || waterBallSpawner == null) return;

        // ------------- actual spawning ------------------//

        // Allows for weapon to communicate to GameManager

        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnSpellFired?.Invoke(SpellTypes.WaterSpell);

        for (int i = 0; i < waterBalls; i++)
        {
            float waterBallAngle = (waterballDiv * i);

            Quaternion rotation = Quaternion.Euler(0, waterBallAngle, 0);

            var waterBullet = Instantiate(waterBallProjectile, waterBallSpawner.transform.position, rotation);

            Rigidbody rb = waterBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = waterBullet.transform.forward * waterBallSpeed;
            }
        }

        waterPulses -= 1;
    }
}
