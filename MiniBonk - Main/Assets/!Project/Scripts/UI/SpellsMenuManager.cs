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

    public void LearnSpell()
    {
        
    }
    
    public void BackToMenu()
    {
        GameManager.Instance.uiManager.UIEvents.OnCloseUIScene(UISceneNames.Spells, false);   
    }

    #endregion
}
