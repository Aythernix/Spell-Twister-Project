using System;
using UnityEngine;

namespace Minibonk.Player
{
    [CreateAssetMenu(fileName = "PlayerID", menuName = "Scriptable Objects/PlayerID")]
    public class PlayerData : ScriptableObject
    {
        // Constants
        [field: SerializeField] private const float MaxHealth = 100;
        [field: SerializeField] private const float MaxStamina = 100;
        
        
        // Max
        public float variableMaxHealth;
        public float variableMaxStamina;
        
        
        public float currentHealth;
        public float currentStamina;
        public float currentMoney;
        public float currentTracePoints;
        public int level;
        public int xp;

        public void Reset()
        {
            currentHealth = MaxHealth;
            currentStamina = MaxStamina;
            level = 1;
            xp = 0;
        }
    } 
    
}

