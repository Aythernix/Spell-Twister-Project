using Minibonk.Management;
using Minibonk.UI;
using TMPro;
using UnityEngine;

public class SpellsMenuManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _tracePointsText;
    
    #endregion
    #region Unity Methods

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tracePointsText.text = $"x{GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetTracePoint?.Invoke()}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Private Methods

    public void LearnSpell(SpellController spellController)
    {
        var spellType = spellController._spellType;
        var cost = spellController._spellCost;
       
        if (GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.IsSpellUnlocked.Invoke(spellType))
        {
            return;
        }

        if (GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetTracePoint?.Invoke() < cost)
        {
            return;
        }
        
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnDecreaseTracePoints?.Invoke(cost);
        GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnUnlockSpell?.Invoke(spellType);
        spellController.CheckIfLearnt();
        
    }
    
    public void BackToMenu()
    {
        GameManager.Instance.uiManager.UIEvents.OnCloseUIScene(UISceneNames.Spells, false);   
    }

    #endregion
}
