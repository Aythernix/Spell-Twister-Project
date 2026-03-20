using UnityEngine;

public class BIGTTIMER : MonoBehaviour
{
    public float _Time = 90f;

    private void Update()
    {
        _Time -= Time.deltaTime;

        if (_Time < 0)
        {
            Destroy(gameObject);
        }
    }
}
