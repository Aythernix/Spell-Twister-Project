using Unity.Mathematics;
using UnityEngine;
using MiniBonk.Enemy;
using Minibonk.Management;

public class FrostBiteDOT : MonoBehaviour
{
    // parameters

    public WeaponDamageStats weaponDamageStats;

    public float zoneDOT_effectDuration = 10f;
    public float zoneDOT_effectTimer; // variable that counts down
    public float lastDOTTick;
    public bool isInsideZone = false;

    private Enemy targetEnemy;
    private Color originalEnemyColour;
    private Renderer enemyRenderer;

    private void Start()
    {
        targetEnemy = GetComponent<Enemy>();
        zoneDOT_effectTimer = zoneDOT_effectDuration;

        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
        {
            originalEnemyColour = enemyRenderer.material.color;

            enemyRenderer.material.color = Color.cyan;
        }
    }

    public void ResetDurationTimer()
    {
        zoneDOT_effectTimer = zoneDOT_effectDuration;
        isInsideZone = true;
    }

    private void Update()
    {
        if (targetEnemy == null) return;

        if (!isInsideZone)
        {
            zoneDOT_effectTimer -= Time.deltaTime;
        }

        lastDOTTick += Time.deltaTime;
        if (lastDOTTick >= 0.5f)
        {
            lastDOTTick = 0;
            targetEnemy.TakeDamage(weaponDamageStats.frostBiteDOT, GameManager.Instance.currentLevelManager.playerHandler.gameObject);
        }

        if (zoneDOT_effectTimer <= 0)
        {
            Destroy(this);
        }  
    }

    private void OnDestroy()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalEnemyColour;
        }
    }

}
