using System;
using Minibonk.Management;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;

public class LevelUpUI : MonoBehaviour
{
    public List<UpgradeData> allUpgrades = new List <UpgradeData>();
    //Script used only for Level up UI
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private TextMeshProUGUI[] titleTexts;
    [SerializeField] private TextMeshProUGUI[] descriptionTexts;

    private void Start()
    {
        //checking if instance exists before subscribing
        if (GameManager.Instance != null && GameManager.Instance.EventHandler != null)
        {
            Debug.Log("LevelingSystem: Successfully subscribed to Events!");
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

    private void ShowLevelUpPanel(int newLevel)
    {
        levelUpPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; //Unlocks the cursor to select buttons
        
        Debug.Log("Level Up Panel Shown for level: " + newLevel);
        
        // ============================================================
        //                    Random Upgrade Selector
        // ============================================================
        List <UpgradeData> tempDeck = new List<UpgradeData>(allUpgrades);
        for (int i = 0; i < titleTexts.Length; i++)
        {
            if (tempDeck.Count == 0) break; // No more upgrades to choose from
            
            int randomIndex = UnityEngine.Random.Range(0, tempDeck.Count);
            UpgradeData selectedUpgrade = tempDeck[randomIndex];
            
            titleTexts[i].text = selectedUpgrade.upgradeName;
            descriptionTexts[i].text = selectedUpgrade.description;
            
            tempDeck.RemoveAt(randomIndex); // Remove selected upgrade to avoid duplicates
        }
    }

    public void CloseMenu()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    [System.Serializable]
    public class UpgradeData
    {
        public string upgradeName;
        public string description;
        //public Sprite icon; 
    }
    
}
