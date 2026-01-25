using System;
using UnityEngine;

namespace Minibonk.Player
{
    [CreateAssetMenu(fileName = "PlayerID", menuName = "Scriptable Objects/PlayerID")]
    public class PlayerData : ScriptableObject
    {
        // CONSTANTS
        public float maxHealth;
        
        public float currentHealth;
        public int level;
        public int Xp;

        private void Reset()
        {
            currentHealth = maxHealth;
            level = 1;
            Xp = 0;
        }
    } 
    
}

