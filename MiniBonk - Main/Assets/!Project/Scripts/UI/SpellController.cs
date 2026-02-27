using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{
    #region Variables
    
    [Header("Text References")]
    [SerializeField] private Image _spellCover;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private GameObject _alreadyLearntText;
    [Space]
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _coolDownText;
    [SerializeField] private TextMeshProUGUI _castsText;
    [SerializeField] private TextMeshProUGUI _speedText;
    
    [Header("Properties")]
    [SerializeField] Sprite _spellIcon;
    [SerializeField] private string _spellName;
    [SerializeField] private Color _spellNameColor;
    [TextArea]
    [SerializeField] private string _spellDescription;
    [SerializeField] private int _spellCost;

    [Header("Stats")]
    [SerializeField] private float _damage;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _casts;
    [SerializeField] private float _speed;
    
    
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    #region Unity Methods

    private void OnValidate()
    {
        //AssignValues();
    }
    void Start()
    {
        AssignValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
    
    #region Private Methods

    private void AssignValues()
    {
        _spellCover.sprite = _spellIcon;
        _name.text = _spellName;
        Color spellColor = _spellNameColor;
        spellColor.a = 1f; // Ensure full opacity
        _name.color = spellColor;
        _description.text = _spellDescription;
        _price.text = $"{_spellCost} Trace Points";
        
        _damageText.text = $"Damage: {_damage}";
        _coolDownText.text = $"Cast Time: {_coolDown} Secs";
        _castsText.text = $"Casts: {_casts}";
        _speedText.text = $"Speed: {_speed} m/s";
    }
    
    #endregion
}
