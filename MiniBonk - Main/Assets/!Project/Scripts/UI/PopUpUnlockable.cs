using System;
using Minibonk.Management;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PopUpUnlockable", menuName = "Scriptable Objects/PopUpUnlockable")]
public class PopUpUnlockable : ScriptableObject
{
    #region Variables

    [Header("Description")]
    public AnimatorController animatorController;
    public string description;
    public string currency;

    [Header("Properties")]
    public int reward;
    public float levelMultiplier;
    public bool moneyIncrease;
    public bool traceIncrease;
    public bool maxHealthIncrease;
    public bool maxStaminaIncrease;
        
    #endregion
    

    #region Optional API

    public void IncreaseMaxStamina(int _reward)
    {
       GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnIncreaseMaxStamina?.Invoke(_reward);
    }
    public void IncreaseMaxHealth(int _reward)
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnIncreaseMaxHealth?.Invoke(_reward);
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnPlayerHeal?.Invoke(_reward);
    }

    public void IncreaseMoney(int _reward)
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnIncreaseMoney?.Invoke(_reward);
    }

    public void IncreaseTrace(int _reward)
    {
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnIncreaseTracePoints?.Invoke(_reward);
    }

    #endregion
}
