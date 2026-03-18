using System;
using Minibonk.Management;
using Minibonk.Player;
using Unity.Cinemachine;
using UnityEngine;

public enum SpellTypes
{
    FireballSpell,
    WaterSpell,
    WindSpell,
    IceSpell,
}

public class WeaponManager : MonoBehaviour
{
    // determins what weapon is currently active
    public int currentWeapon = 1;

    // Weapon scripts

    [SerializeField] private Weapon_FireType fireWeapon; // Type 1
    [SerializeField] private Weapon_WaterType waterWeapon; // Type 2
    [SerializeField] private Weapon_WindType windWeapon; // Type 3
    [SerializeField] private Weapon_IceType iceWeapon; // Type 4

    // Variables for Player Movement Buffs
    public GameObject player;
    private PlayerMovement _playerMovement;
    private float _originalWalkSpeed;
    private float _originalJumpForce;

    private float _walkSpeedToAdd = 8f;
    private float _jumpForceToAdd = 4f;

    private bool _isWindBuffActive = false;

    // FOV changes when buff is active
    [SerializeField] private CinemachineCamera _virtualCamera;
    private float _originalFOV;
    private float _fovToAdd = 10f;
    private float _fovLerpSpeed = 5f;
    private bool _fovCaptured = false;

    private void Start()
    {
        _playerMovement = player.GetComponent<PlayerMovement>();
        _originalWalkSpeed = _playerMovement._walkSpeed;
        _originalJumpForce = _playerMovement._jumpForce;

        if (_virtualCamera != null)
        {
            _originalFOV = _virtualCamera.Lens.FieldOfView;

            if (_originalFOV <= 0) _originalFOV = 60f;
        }
        
    }

    private void OnEnable()
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.IsSpellEquipped += GetCurrentWeapon;
    }

    private void OnDisable()
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.IsSpellEquipped -= GetCurrentWeapon;
    }

    public void Update()
    {
        if (!_fovCaptured && _virtualCamera != null)
        {
            _originalFOV = _virtualCamera.Lens.FieldOfView;
            if (_originalFOV > 0) _fovCaptured = true;
        }

        // Runs Buff Check
        HandleMovementBuffs();

        // Camera FOV adjustments
        if (_virtualCamera != null && _fovCaptured)
        {
            float targetFOV = _isWindBuffActive ? (_originalFOV + _fovToAdd) : _originalFOV;

            var lens = _virtualCamera.Lens;
            lens.FieldOfView = Mathf.Lerp(lens.FieldOfView, targetFOV, Time.deltaTime * _fovLerpSpeed);
            _virtualCamera.Lens = lens;
        }
        

        switch (currentWeapon)
        {
            case 1:
                fireWeapon.shootFireBall();
                break;
            case 2:
                waterWeapon.ShootWaterPulse();
                break;
            case 3:
                windWeapon.ShootWindBlade();
                break;
            case 4:
                iceWeapon.ShootIceProjectile();
                break;
            default:
                Debug.Log("Not a weapon index!");
                break;
        }
    }

    private void HandleMovementBuffs()
    {
        if (currentWeapon == 3 && !_isWindBuffActive)
        {
            _playerMovement._walkSpeed += _walkSpeedToAdd;
            _playerMovement._jumpForce += _jumpForceToAdd;
            _isWindBuffActive = true;
        }
        else if (currentWeapon != 3 && _isWindBuffActive)
        {
            _playerMovement._walkSpeed = _originalWalkSpeed;
            _playerMovement._jumpForce = _originalJumpForce;
            _isWindBuffActive = false;
        }
   
    }

    private bool GetCurrentWeapon(SpellTypes spellType)
    {
        return (int)spellType + 1 == currentWeapon;
    }
}
