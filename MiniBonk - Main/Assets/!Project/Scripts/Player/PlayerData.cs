using UnityEngine;

namespace Minibonk.Player
{
    [CreateAssetMenu(fileName = "PlayerID", menuName = "Scriptable Objects/PlayerID")]
    public class PlayerData : ScriptableObject
    {
        public readonly float maxHealth;
        public float currentHealth;
    } 
}

