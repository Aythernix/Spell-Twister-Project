using UnityEngine;

public class WaterFlowAnim : MonoBehaviour
{
    [Header("Scroll Speeds")]
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;

    [Header("Bobbing Speeds")]
    public float bobHeight = 0.2f;
    public float bobSpeed = 1f;

    private Renderer rend;
    private Vector3 bobStartPos;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        bobStartPos = transform.localPosition;
    }

    private void Update()
    {
        // Scrolls the texture
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));

        // Makes the water 'Bob'
        float newY = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = bobStartPos + new Vector3(0, newY, 0);
    }
}
