using Minibonk.Player;
using Unity.Mathematics;
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
    private int PlayerXp
    {
        get => _playerHandler.playerData.Xp;
        set => _playerHandler.playerData.Xp = value;
    }

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
        PlayerXp += amount;

        while (PlayerXp >= RequiredXP(PlayerLevel))
        {
            int cost = RequiredXP(PlayerLevel);
            PlayerXp -= cost;
            OnLevelIncrement(); 
        }
    }

    // Called when the player level increases
    private void OnLevelIncrement( int levelAmount = 1)
    {
        PlayerLevel += levelAmount;
    }

    // Calculates the amount of XP the player should need to level up
    // Dependent on the players current level
    private int RequiredXP(int playerLevel)
    {
        return Mathf.RoundToInt(_baseRequiredXp * Mathf.Pow(_XpMultipler, playerLevel - 1));
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
