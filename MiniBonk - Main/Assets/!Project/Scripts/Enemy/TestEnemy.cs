using UnityEngine;
using MiniBonk.Enemy;
using Minibonk.Management;
using Minibonk.Player;

public class TestEnemy : Enemy 
{
    protected override void Attack() 
    {
        if (player.TryGetComponent <PlayerHandler>(out PlayerHandler handler))
        {
            Debug.Log($"Enemy dealt {data.damage * GameManager.Instance.ChaosMultiplier()}");
            handler.PlayerEvents.OnPlayerDamaged?.Invoke(data.damage * GameManager.Instance.ChaosMultiplier()); // "?" used to ensure there are subscribers and prevent Null crash if no subs
        }
        Debug.Log("I am attacking the player!");
    }
}
