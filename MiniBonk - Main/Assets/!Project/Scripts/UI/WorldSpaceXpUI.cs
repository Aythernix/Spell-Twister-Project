using System;
using MiniBonk.Enemy;
using Minibonk.Management;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WorldSpaceXpUI : MonoBehaviour
{
    [SerializeField] GameObject _damageTextPrefab;
    [SerializeField] private Color _damageTextColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameManager.Instance.EventHandler.OnEnemyKilled += CatchEnemyDeath;
    }
    private void OnDisable()
    {
        GameManager.Instance.EventHandler.OnEnemyKilled -= CatchEnemyDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CatchEnemyDeath(GameObject killer, GameObject victim)
    {
        ShowNewDamageText(victim.GetComponent<Enemy>().data.xpValue.ToString());
    }

    private void ShowNewDamageText(string text)
    {
        var textObj = Instantiate(_damageTextPrefab, transform, false);
        textObj.GetComponent<TextMeshPro>().color = _damageTextColor;
        textObj.GetComponent<TextMeshPro>().text = text;
    }
}
