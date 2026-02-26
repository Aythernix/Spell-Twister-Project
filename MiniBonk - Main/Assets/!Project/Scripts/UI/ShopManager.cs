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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moneyText.text = $"x{GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetMoney?.Invoke()}";
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
        }
        
    }

    #endregion
}
