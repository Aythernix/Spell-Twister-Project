using Minibonk.Player;
using UnityEngine;

public class LevelingSystem : PlayerSystem
{
    #region Varibles

    [Header("XP Configuration")]
    [SerializeField] private int _baseRequiredXp = 100;
    [SerializeField] private float _XpMultipler = 1.1f;

    // Player Level Data
    private int PlayerLevel
    {
        get => _playerHandler.playerData.level;
        set => _playerHandler.playerData.level = value;
    }
    private int _playerXp;

    #endregion

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    
    #region Private Methods

    // Called when the XP count gets increased
    private void OnIncreaseXP(int amount)
    {
        _playerXp += amount;

        if (_playerXp >=  RequiredXP())
        {
            OnLevelIncrement();
        }
        // NOTE: Doesn't take into account the amount of incoming XP
        // For Example, if the requirement is 100, and the player gets 200 XP
        // The players level will increase by 1 not 2
    }

    // Called when the player level increases
    private void OnLevelIncrement(int amount = 1)
    {
        PlayerLevel += amount;
        
    }

    // Calculates the amount of XP the player should need to level up
    // Dependent on the players current level
    private int RequiredXP()
    {
        if (PlayerLevel == 1)
        {
            return _baseRequiredXp;
        }
        return Mathf.RoundToInt(_baseRequiredXp * Mathf.Pow(_XpMultipler, PlayerLevel));
    }
    
    #endregion

    #region PublicAPI

    /// <summary>
    ///   Add XP to the players current XP counter
    /// </summary>
    /// <param name="amount"> the amount of XP to add</param>
    public void AddXp(int amount)
    {
        OnIncreaseXP(amount);
    }

    #endregion
}
