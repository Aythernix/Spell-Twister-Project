using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeDetector : MonoBehaviour
{
    // Creates a list that will be used to store enemies within range

    public List<Transform> enemiesWithinRange = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // adds enemy position to our list when its inside the trigger hitbox
            enemiesWithinRange.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Removes enemy position from our list when it leaves the trigger hitbox
            enemiesWithinRange.Remove(other.transform);
        }
    }

    public Transform CheckForClosestEnemy()
    {
        // sets the closestEnemy variable to null as there wont be a target on startup
        Transform closestEnemy = null;

        // sets the default range of the closest distance to our enemy to infinty. This will make it easier to check which enemy position is closer to the player later on
        float closestEnemyDistance = Mathf.Infinity;

        // creates a vector 3 with our players position so we can compare enemy positions to the players
        Vector3 playerPos = transform.position;

        // will loop through all current transforms in our list
        foreach (Transform enemyPos in enemiesWithinRange)
        {
            // creates a vector 3 from the player to an enemy in the list
            Vector3 distanceFromEnemy = enemyPos.position - playerPos;

            // gets the rough vector from our vector3 above and assigns it to the enemyRange variable
            float enemyRange = distanceFromEnemy.sqrMagnitude;

            // if statement that checks if an enemy position is closer to the player than the last
            if (enemyRange < closestEnemyDistance)
            {
                // sets the closest enemy in our lists position to the 'closestEnemyDistance' transform
                closestEnemyDistance = enemyRange;

                // assigns the closest enemies position to the 'closestEnemy' variable to be used
                closestEnemy = enemyPos;
            }
        }
        return closestEnemy;
    }
}
