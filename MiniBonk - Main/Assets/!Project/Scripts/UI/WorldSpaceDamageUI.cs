using System;
using Minibonk.Management;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WorldSpaceDamageUI : MonoBehaviour
{
    [SerializeField] GameObject _damageTextPrefab;
    [SerializeField] private Color _damageTextColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameManager.Instance.EventHandler.OnEnemyDamaged += CatchEnemyDamage;
    }
    private void OnDisable()
    {
        GameManager.Instance.EventHandler.OnEnemyDamaged -= CatchEnemyDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CatchEnemyDamage(float amount, GameObject victim)
    {
        ShowNewDamageText(amount.ToString());
    }

    private void ShowNewDamageText(string text)
    {
        var textObj = Instantiate(_damageTextPrefab, transform, false);
        textObj.GetComponent<TextMeshPro>().color = _damageTextColor;
        textObj.GetComponent<TextMeshPro>().text = text;
    }
}
