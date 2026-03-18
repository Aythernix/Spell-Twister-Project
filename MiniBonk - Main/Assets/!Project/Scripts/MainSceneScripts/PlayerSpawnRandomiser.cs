using UnityEngine;

public class PlayerSpawnRandomiser : MonoBehaviour
{
    public Transform[] spawners;
    public GameObject player;

    private void Start()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (spawners.Length == 0) return;

        int randomIndex = Random.Range(0, spawners.Length);
        player.transform.position = spawners[randomIndex].position;
        player.transform.rotation = spawners[randomIndex].rotation;

        Physics.SyncTransforms();

        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}
