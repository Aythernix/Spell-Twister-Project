using Minibonk.Management;
using UnityEngine;
using Minibonk.Management;
public class DebugCheats : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance==null) return;
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Cheat Activated: Add XP [50]");
            
            GameManager.Instance.EventHandler.AddXp?.Invoke(50);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("Forcing Level up..");
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease?.Invoke(1);
           
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("Cheat Activated: Unlocked All Weapons");
            GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnUnlockSpell?.Invoke(SpellTypes.WindSpell);
            GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnUnlockSpell?.Invoke(SpellTypes.IceSpell);
            GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnUnlockSpell?.Invoke(SpellTypes.WaterSpell);
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("Cheat Activated: Added 10k currencies");
            GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnIncreaseMoney?.Invoke(10000);
            GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnIncreaseTracePoints?.Invoke(10000);
            
        }

    }
}
