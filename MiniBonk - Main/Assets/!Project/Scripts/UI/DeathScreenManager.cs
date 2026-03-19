using Minibonk.Management;
using Minibonk.UI;
using TMPro;
using UnityEngine;

public class DeathScreenManager : MonoBehaviour
{

    [SerializeField]private TextMeshPro killCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetKillCount();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        GameManager.Instance.uiManager.UIEvents.SwapActiveUIScene?.Invoke(UISceneNames.MainMenu);
    }
    
    private void SetKillCount()
    {
        killCountText.text = GameManager.Instance.killCount.ToString();
    }
}
