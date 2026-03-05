using System;
using System.Collections;
using Minibonk.Management;
using Minibonk.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotController : MonoBehaviour
{
    #region Variables
    
    [Header("Properties")]
    [SerializeField] private float _KeybindScaleAmount = 1.3f;
    [SerializeField] private float _KeybindScaleSpeed = 5f;
    [SerializeField] private float _SpriteScaleAmount = 1.1f;
    [SerializeField] private float _SpriteScaleSpeed = 5f;
    [SerializeField] private SpellTypes _spellTypes;

    [Header("References")]
    [SerializeField] private Image _weaponCoverImage;
    [SerializeField] private Image _Keybind;
    
    private Coroutine _clickCoroutine;
    private Coroutine _fireCoroutine;

    #endregion
    
    #region Unity Methods

    private void OnEnable()
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnUnlockSpell += Unlock;
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnEquipSpell += SpellEquipped;
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnSpellFired += SpellFired;
    }

    private void OnDisable()
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnUnlockSpell -= Unlock;
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnEquipSpell -= SpellEquipped;
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnSpellFired -= SpellFired;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _weaponCoverImage.enabled = false;
        if (GameManager.Instance.currentLevelManager.playerHandler.GetComponent<PlayerWeaponHandler>().CheckIfSpellUnlocked(_spellTypes))
        {
            _weaponCoverImage.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
    
    private IEnumerator VisualiseClick()
    {
        // Displays the slot as active
            
        _Keybind.transform.localScale = new Vector3(1, 1, 1) * _KeybindScaleAmount;
        yield return new WaitForSeconds(0.1f);
        _Keybind.transform.localScale = new Vector3(1, 1, 1);
    }
    
    private IEnumerator VisualiseFire()
    {
        // Displays the slot as active
            
        _weaponCoverImage.transform.localScale = new Vector3(1, 1, 1) * _SpriteScaleAmount;
        yield return new WaitForSeconds(0.1f);
        _weaponCoverImage.transform.localScale = new Vector3(1, 1, 1);
    }

    private void SpellEquipped(SpellTypes spellType)
    {
        if (_spellTypes != spellType) return;

        if (_clickCoroutine != null)
        {
            StopCoroutine(_clickCoroutine);
        }
        
        _clickCoroutine = StartCoroutine(VisualiseClick());
    }

    private void SpellFired(SpellTypes spellType)
    {
        if (_spellTypes != spellType) return;

        if (_fireCoroutine != null)
        {
            StopCoroutine(_fireCoroutine);
        }
        
        _fireCoroutine = StartCoroutine(VisualiseFire());
    }

    private void Unlock(SpellTypes spellType)
    {
        if (_spellTypes != spellType) return;
        
        _weaponCoverImage.enabled = true;
    }
}
