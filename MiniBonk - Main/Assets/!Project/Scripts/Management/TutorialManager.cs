using System;
using System.Collections;
using Minibonk.Management;
using Minibonk.UI;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _walkingPrompt;
    [SerializeField] private GameObject _levelUpPrompt;
    [SerializeField] private GameObject _shopPrompt;
    
    // Bool flags
    private bool _shownWalkPrompt;
    private bool _shownLevelUpPrompt;
    private bool _shownShopPrompt;
    
    #endregion

    private void OnEnable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease += ShowLevelUpPrompt;
        GameManager.Instance.uiManager.UIEvents.OnInstantiateUIScene += ShowShopPrompt;
    }
    private void OnDisable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease -= ShowLevelUpPrompt;
        GameManager.Instance.uiManager.UIEvents.OnInstantiateUIScene -= ShowShopPrompt;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (!GameManager.Instance.gameSettings.showTutorial)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            ShowWalkingPrompt();
            GameManager.Instance.gameSettings.showTutorial = false;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowWalkingPrompt()
    {
        if (_shownWalkPrompt) return;
        try
        {
            _walkingPrompt.SetActive(true);
            _shownWalkPrompt = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void HideWalkingPrompt()
    {
        try
        {
            _walkingPrompt.GetComponent<Animator>().Play("Close Anim");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void ShowLevelUpPrompt(int level)
    {
        if (_shownLevelUpPrompt) return;
        _shownLevelUpPrompt = true;
        if (level == 2)
        {
            try
            {
                _levelUpPrompt.SetActive(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    public void HideLevelUpPrompt()
    {
        try
        {
            _levelUpPrompt.GetComponent<Animator>().Play("Close Anim");
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void ShowShopPrompt(UISceneNames scene, bool pause)
    {
        if (scene != UISceneNames.Shop) return;
        if (_shownShopPrompt) return;
        try
        {
            _shopPrompt.SetActive(true);
            _shownShopPrompt = true;
            StartCoroutine(HideShopPrompt());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public IEnumerator HideShopPrompt()
    {
        yield return new WaitForSecondsRealtime(3f);
        try
        {
            _shopPrompt.GetComponent<Animator>().Play("Close Anim");
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
