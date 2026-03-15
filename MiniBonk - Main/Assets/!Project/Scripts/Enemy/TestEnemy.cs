using UnityEngine;
using MiniBonk.Enemy;
using Minibonk.Management;
using Minibonk.Player;

public class TestEnemy : Enemy 
{
    private PlayerHandler playerHandler; // Cached PlayerHandler
    
    protected override void Start()
    {
        base.Start();
        player.TryGetComponent<PlayerHandler>(out playerHandler);
    }
    
    public override void Attack() 
    {
        if (playerHandler != null)
        {
            Debug.Log($"Enemy dealt {data.damage * GameManager.Instance.ChaosMultiplier()}");
            playerHandler.PlayerEvents.OnPlayerDamaged?.Invoke(data.damage * GameManager.Instance.ChaosMultiplier()); 
        }
        // Debug.Log("I am attacking the player!");
    }
}
