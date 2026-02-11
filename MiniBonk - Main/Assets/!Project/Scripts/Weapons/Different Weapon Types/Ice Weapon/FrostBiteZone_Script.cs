using UnityEngine;
using MiniBonk.Enemy;
using Unity.VisualScripting;

public class FrostBiteZone_Script : MonoBehaviour
{
    // -------------- Script for the Dot zone to add the effect ---------------------//

    public float zoneLifeTime = 10f;
    public WeaponDamageStats weaponDamageStats;


    private void Update()
    {
        zoneLifeTime -= Time.deltaTime;

        if (zoneLifeTime <= 0) Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<FrostBiteDOT>(out FrostBiteDOT existingDOT))
            {
                existingDOT.ResetDurationTimer();
            }
            else
            {
                FrostBiteDOT newDOT = other.gameObject.AddComponent<FrostBiteDOT>();
                newDOT.weaponDamageStats = this.weaponDamageStats;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<FrostBiteDOT>(out FrostBiteDOT existingDOT))
            {
                existingDOT.isInsideZone = false;
            }
        }
    }
}
