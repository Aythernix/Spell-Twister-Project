using Minibonk.Player;
using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    #region Variables

    public PlayerData playerData;
    public Events events;
    
   

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        events = new Events();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct Events
{
    public Action<Vector2> OnMove;
    public Action OnJumpStarted;
    public Action<int> OnAbilityPressed;
    public Action<int> OnAbilityReleased;
}
