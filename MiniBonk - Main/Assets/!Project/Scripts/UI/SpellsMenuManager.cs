using Minibonk.Management;
using Minibonk.UI;
using UnityEngine;

public class SpellsMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LearnSpell()
    {
        
    }
    
    public void BackToMenu()
    {
        GameManager.Instance.uiManager.UIEvents.OnCloseUIScene(UISceneNames.Spells, false);   
    }
}
