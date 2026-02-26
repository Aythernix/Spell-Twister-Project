using Minibonk.Management;
using Minibonk.MovementAbilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopOptionController : MonoBehaviour
{
    #region Variables

    public MovementAbility _movementAbilitySO;
    
    [SerializeField] private Animator _cover;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
    

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_movementAbilitySO == null) return;
        _cover.runtimeAnimatorController = _movementAbilitySO.coverAnim;
        _name.text = _movementAbilitySO.displayName;
        _description.text = _movementAbilitySO.abilityDescription;
        CheckIfCanAfford();
        CheckIfPurchased();
        Debug.Log(CheckIfPurchased());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfPurchased()
    {
        if (GameManager.Instance.currentLevelManager.playerHandler.GetComponent<AbilityManager>().unlockedAblityIDs.Contains(_movementAbilitySO.abilityID))
        {
            _buyButtonText.text = $"Purchased";
            return true;
        }
        
        _buyButtonText.text = $"buy {_movementAbilitySO.buyName}";
        return false;
    }

    public bool CheckIfCanAfford()
    {
        if (GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.GetMoney?.Invoke() >= _movementAbilitySO.cost)
        {
            _price.text = $"Cost {_movementAbilitySO.cost}";
            return true;
        }
        _price.text = $"Cost {_movementAbilitySO.cost} (Can't Afford)";
        return false;
    }
}
