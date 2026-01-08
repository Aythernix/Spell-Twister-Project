using UnityEngine;
using MiniBonk.Enemy;
public class TestEnemy : Enemy 
{
    public override void Attack() 
    {
        Debug.Log("I am attacking the player!");
    }
}
