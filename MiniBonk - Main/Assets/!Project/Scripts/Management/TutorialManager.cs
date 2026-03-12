using System;
using Minibonk.Management;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _walkingPrompt;
    [SerializeField] private GameObject _levelUpPrompt;
    
    #endregion

    private void OnEnable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease += ShowLevelUpPrompt;
    }
    private void OnDisable()
    {
        GameManager.Instance.EventHandler.OnPlayerLevelIncrease -= ShowLevelUpPrompt;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowWalkingPrompt()
    {
        try
        {
            _walkingPrompt.SetActive(true);
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
}
