using Minibonk.Player;
using System;
using UnityEngine;

namespace Minibonk.Player
{
    // TODO Add Comments and summaries
    public class PlayerHandler : PlayerSystem
    {
        #region Variables

        public PlayerData playerData;
        public readonly Events Events = new();
        #endregion

        #region Unity Methods
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Start()
        {
            playerData.Reset();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        #endregion
        
    }

    public class Events
    {
        public Action<Vector2> OnMove;
        public Action OnJumpStarted;
        public Action<int> OnAbilityPressed;
        public Action<int> OnAbilityReleased;
        public Action<float> OnPlayerDamaged;
    }

}
