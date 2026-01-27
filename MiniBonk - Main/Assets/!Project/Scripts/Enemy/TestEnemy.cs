using UnityEngine;
using MiniBonk.Enemy;
using Minibonk.Player;

public class TestEnemy : Enemy 
{
    protected override void Attack() 
    {
        if (player.TryGetComponent<PlayerHandler>(out PlayerHandler handler))
        {
            handler.Events.OnPlayerDamaged?.Invoke(data.damage); // "?" used to ensure there are subscribers and prevent Null crash if no subs
        }
        Debug.Log("I am attacking the player!");
    }
}
