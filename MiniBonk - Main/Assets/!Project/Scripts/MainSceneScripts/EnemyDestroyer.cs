using UnityEngine;
using Minibonk.Enemy;
using MiniBonk.Enemy;

public class EnemyDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Die(null);
        }
    }
}
