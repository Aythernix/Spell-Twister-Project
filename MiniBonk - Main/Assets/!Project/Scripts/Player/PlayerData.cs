using System;
using UnityEngine;

namespace Minibonk.Player
{
    [CreateAssetMenu(fileName = "PlayerID", menuName = "Scriptable Objects/PlayerID")]
    public class PlayerData : ScriptableObject
    {
        // Max
        public float maxHealth;
        public float maxStamina;
        
        
        public float currentHealth;
        public float currentStamina;
        public int level;
        public int xp;

        public void Reset()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            level = 1;
            xp = 0;
        }
    } 
    
}

