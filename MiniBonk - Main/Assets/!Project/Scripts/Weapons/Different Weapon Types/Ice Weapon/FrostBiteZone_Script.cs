using System;
using UnityEngine;
using MiniBonk.Enemy;
using System.Collections.Generic;

public class FrostBiteZone_Script : MonoBehaviour
{
    // -------------- Script for the Dot zone to add the effect ---------------------//

    public float zoneLifeTime = 10f;
    public WeaponDamageStats weaponDamageStats;
    
    //Visuals
    [SerializeField] private Color frostColor = new Color(0f, 0.6f ,1f, 1f);
    
    private List<Enemy> affectedEnemies = new List<Enemy>();
    private MaterialPropertyBlock propBlock;
    private static readonly int Color = Shader.PropertyToID("_BaseColor");

    private void Start()
    {
        propBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        zoneLifeTime -= Time.deltaTime;

        if (zoneLifeTime <= 0) Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            ApplyFrostEffect(enemy);
            if (other == null) return;

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
    private void ApplyFrostEffect(Enemy enemy)
    {
        foreach (var rend in enemy.model)
        {
            if (rend == null) continue;

            rend.GetPropertyBlock(propBlock);
            propBlock.SetColor(Color, frostColor);
            rend.SetPropertyBlock(propBlock);
        }
    }
    private void RemoveFrostEffect(Enemy enemy)
    {
        foreach (var rend in enemy.model)
        {
            if (rend == null) continue;

            rend.GetPropertyBlock(propBlock);
            propBlock.Clear();
            rend.SetPropertyBlock(propBlock);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent<Enemy>(out Enemy enemy))
            RemoveFrostEffect(enemy);
        {
            if (other == null) return;

            if (other.TryGetComponent<FrostBiteDOT>(out FrostBiteDOT existingDOT))
            {
                existingDOT.isInsideZone = false;
            }
        }
    }
}
