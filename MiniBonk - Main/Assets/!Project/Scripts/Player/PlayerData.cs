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
        public int currentMoney;
        public int currentTracePoints;
        public int level;
        public int xp;

        public void Reset()
        {
            variableMaxHealth = MaxHealth;
            variableMaxStamina = MaxStamina;
            currentHealth = MaxHealth;
            currentStamina = MaxStamina;
            currentMoney = 0;
            currentTracePoints = 0;
            level = 1;
            xp = 0;
        }
    } 
    
}

