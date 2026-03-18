using UnityEngine;

public class WaterRippleSpawner : MonoBehaviour
{
    public GameObject ripplePrefab;
    public float _spawnRate = 0.2f;
    private float _nextSpawnTime;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water") && Time.time > _nextSpawnTime)
        {
            SpawnRipple(other);
            _nextSpawnTime = Time.time + _spawnRate;
        }
    }

    void SpawnRipple(Collider water)
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        // Check if that ray hits the water
        if (Physics.Raycast(ray, out hit, 1.85f))
        {
            Vector3 spawnPos = hit.point + new Vector3(0, 0.01f, 0);

            GameObject newRipple = Instantiate(ripplePrefab, spawnPos, Quaternion.Euler(90, 0, 0));

            // Parent it so it bobs
            newRipple.transform.SetParent(water.transform, true);
        }
    }
}
