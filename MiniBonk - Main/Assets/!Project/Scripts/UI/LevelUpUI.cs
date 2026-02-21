using Minibonk.Management;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Minibonk.MovementAbilities;

public class LevelUpUI : MonoBehaviour
{
    
    #region Variables
    
    private List<MovementAbility> currentChoices = new List<MovementAbility>();
    
    //Script used only for Level up UI
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private TextMeshProUGUI[] titleTexts;
    [SerializeField] private TextMeshProUGUI[] descriptionTexts;
    #endregion
    private void Start()
    {
        //checking if instance exists before subscribing
        if (GameManager.Instance != null && GameManager.Instance.EventHandler != null)
        {
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease += ShowLevelUpPanel; 
        }
        else
        {
            Debug.LogError("Failed to find GameManager! [LEVEL-UP-UI]");
        }
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
        
    }
    

    private void OnDisable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease -= ShowLevelUpPanel;
    }
    // ============================================================
    //                    Display logic for Level Up Panel
    // ============================================================
    private void ShowLevelUpPanel(int newLevel)
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
            Debug.Log("LevelUpUI: Panel GameObject set to Active.");
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; //Unlocks the cursor to select buttons
        
        GenerateUpgradesFromManager();
    }

    public void CloseMenu()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void GenerateUpgradesFromManager()
    {
        currentChoices.Clear();

        var manager = FindFirstObjectByType<AbilityManager>();
        if (manager == null) return;
        
        List<MovementAbility> tempList = new List<MovementAbility>(manager.allAvailAbilities);

        for (int i = 0; i < titleTexts.Length; i++)
        {
            if (tempList.Count == 0) break;
            int randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            MovementAbility ability = tempList[randomIndex];
            
            currentChoices.Add(ability);
            
            titleTexts[i].text = ability.abilityName;
            descriptionTexts[i].text = ability.abilityDescription;
            
            tempList.RemoveAt(randomIndex);
        }
    }
    public void SelectUpgrade(int index)
    {
        if (index >= currentChoices.Count) return;

        MovementAbility ability = currentChoices[index];
        Debug.Log($"LevelUPUI: Player Selected:{ability.abilityName}");
        
        var manager = FindFirstObjectByType<AbilityManager>();
        if (manager != null)
        {
            manager.UnlockAbility(ability.abilityID);
        }
        else
        {
            Debug.LogError("Failed to find AbilityManager! [LEVEL-UP-UI]");
        }
        CloseMenu();
    }

    [System.Serializable]
    public class UpgradeData
    {
        public MovementAbility abilitySO;
        //Future weaopon slot..
        // public WeaponData weaponSO;
        public string upgradeName => abilitySO != null ? abilitySO.abilityName : "Unknown Ability";
        public string description;
        //public Sprite icon;
        
        
    }
    
}
