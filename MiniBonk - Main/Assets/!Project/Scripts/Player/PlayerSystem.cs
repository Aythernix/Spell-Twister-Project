using UnityEngine;

public abstract class PlayerSystem : MonoBehaviour
{
    protected PlayerHandler _playerHandler;
    protected virtual void Awake()
    {
        _playerHandler = transform.root.GetComponent<PlayerHandler>();
        Debug.Log("ran");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
