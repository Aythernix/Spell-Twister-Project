using Minibonk.Management;
using UnityEngine;
public class DebugCheats : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance==null) return;
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Cheat Activated: Add XP [50]");
            
            GameManager.Instance.EventHandler.AddXp?.Invoke(50);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Forcing Level up..");
            GameManager.Instance.EventHandler.OnPlayerLevelIncrease?.Invoke(1);
           
        }

    }
}
