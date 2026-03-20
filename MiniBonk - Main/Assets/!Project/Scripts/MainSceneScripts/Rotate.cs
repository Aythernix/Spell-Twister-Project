using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 obj = new Vector3(0, 160, 0);

    private void Update()
    {
        transform.Rotate(obj * Time.deltaTime);
    }
}
