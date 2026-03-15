using System;
using Minibonk.Management;
using Minibonk.MovementAbilities;
using Minibonk.UI;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI _moneyText;

    #endregion

    private void OnEnable()
    {
        GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerMoneyUI += UpdateMoney;
    }

    private void OnDisable()
    {
        GameManager.Instance.uiManager.UIEvents.OnUpdatePlayerMoneyUI -= UpdateMoney;
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

    private void UpdateMoney(int money)
    {
        _moneyText.text = $"x{money}";
    }

    #region Button Methods

    public void BackToMenu()
    {
        GameManager.Instance.uiManager.UIEvents.OnCloseUIScene(UISceneNames.Shop, false);
    }

    public void Buy(ShopOptionController shopOptionController)
    {
        if (shopOptionController.CheckIfCanAfford())
        {
            if (!shopOptionController.CheckIfPurchased())
            {
                GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnDecreaseMoney(shopOptionController._movementAbilitySO.cost);
                GameManager.Instance.currentLevelManager.playerHandler.GetComponent<AbilityManager>().UnlockAbility(shopOptionController._movementAbilitySO.abilityID);
                shopOptionController.CheckIfPurchased();
                GameManager.Instance.uiManager.UIEvents.UpdateUI?.Invoke();
            }
        }
        
    }

    #endregion
}
