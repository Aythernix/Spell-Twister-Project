using UnityEngine;


namespace Minibonk.MovementAbilities
{
    public abstract class MovementAbility : ScriptableObject
    {
        public string AbilityName;
        public float Cooldown;

        public abstract void Initialize(GameObject player);
        public abstract void Execute();
        public abstract void Finalise();
    }
}

