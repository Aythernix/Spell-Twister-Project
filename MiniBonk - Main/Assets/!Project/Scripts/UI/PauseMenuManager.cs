using Minibonk.Management;
using Minibonk.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] TextMeshProUGUI _tracePointsText;

    #endregion
    #region Unity Events

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moneyText.text = $"x{GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetMoney?.Invoke()}";
        _tracePointsText.text = $"x{GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetTracePoint?.Invoke()}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Button Methods

    public void ResumeGameButton()
    {
        GameManager.Instance.UnPauseGame();
    }

    public void ShopButton()
    {
        GameManager.Instance.uiManager.UIEvents.OnInstantiateUIScene?.Invoke(UISceneNames.Shop, false);
    }

    public void SpellsButton()
    {
        GameManager.Instance.uiManager.UIEvents.OnInstantiateUIScene?.Invoke(UISceneNames.Spells, false);
    }

    public void SettingsButton()
    {
        GameManager.Instance.uiManager.UIEvents.OnInstantiateUIScene?.Invoke(UISceneNames.Settings, false);
    }

    public void MainMenu()
    {
        GameManager.Instance.uiManager.UIEvents.SwapActiveUIScene?.Invoke(UISceneNames.MainMenu);
    }
    

    #endregion
}
