using System;
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

    private void OnEnable()
    {
        GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerMoneyUI += UpdateMoneyUI;
        GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerTracePoints += UpdateTracePointsUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerMoneyUI -= UpdateMoneyUI;
        GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerTracePoints -= UpdateTracePointsUI;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.uiManager.UIEvents.UpdateUI?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
    
    private void UpdateMoneyUI(int money)
    {
        _moneyText.text = $"x{money}";
    }

    private void UpdateTracePointsUI(int tracePoints)
    {
        _tracePointsText.text = $"x{tracePoints}";
    }

    #region Button Methods

    public void ResumeGameButton()
    {
        GameManager.Instance.ClosePauseMenu();
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
