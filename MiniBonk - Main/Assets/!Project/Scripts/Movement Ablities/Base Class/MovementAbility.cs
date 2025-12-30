using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public abstract class MovementAbility : ScriptableObject
    {
        #region Variables
        
        [Header("Generic Settings")]
        public string abilityName;
        public float cooldownTime;
        public float staminaCost;

        protected Rigidbody _rb;
        protected GameObject _player;

        private float _nextReadyTime;
        
        #endregion

        #region Virtual/Abstract Methods

        public virtual void Initialize(GameObject player)
        {
            _player = player;
            _rb = player.GetComponent<Rigidbody>();
        }

        protected abstract void OnExecute();
        
        public virtual void Finalise() {}

        #endregion

        #region Public API

        public virtual void Execute()
        {
            if (!IsOnCooldown)
            {
                StartCooldown();
                OnExecute(); 
            }
        }

        public void StartCooldown()
        {
            _nextReadyTime = Time.time + cooldownTime;
        }

        public bool IsOnCooldown => Time.time < _nextReadyTime;

        
        public float CooldownNormalized => Mathf.Clamp01((_nextReadyTime - Time.time) / cooldownTime);

        #endregion
    }
}