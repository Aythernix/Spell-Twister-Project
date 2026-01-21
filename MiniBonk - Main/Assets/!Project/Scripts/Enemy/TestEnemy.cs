using UnityEngine;
using MiniBonk.Enemy;
public class TestEnemy : Enemy 
{
    protected override void Attack() 
    {
        Debug.Log("I am attacking the player!");
    }
}
