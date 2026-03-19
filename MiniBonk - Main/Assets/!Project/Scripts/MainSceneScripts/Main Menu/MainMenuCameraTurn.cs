using UnityEngine;

public class MainMenuCameraTurn : MonoBehaviour
{
    //private float _turnSpeed = 1.0f;
    public Vector3 rotationSpeed = new Vector3(0, 10, 0);

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
