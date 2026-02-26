using Minibonk.Management;
using Minibonk.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    #region Variables

    //

    #endregion
    #region Unity Events

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
