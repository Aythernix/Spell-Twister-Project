using Minibonk.Management;
using Minibonk.UI;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CLoseCreditsClicked()
    {
        GameManager.Instance.uiManager.CloseUIScene(UISceneNames.Credits, false);
    }
}
