using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    public abstract class MovementAbility : ScriptableObject
    {
        #region Variables
        [Header("Generic Settings")]
        public string abilityName;
        public int abilityID;
        public string abilityDescription;
        public float cooldownTime;
        
        protected float _staminaCost;
        protected float _staminaLossRate;

        public bool autoFinalise { get; protected set; } = false;

        protected Rigidbody _rb;
        protected GameObject _player;

        protected float _nextReadyTime;
        #endregion

        #region Initialization
        /// <summary>
        /// Links the ScriptableObject asset to the physical player in the scene.
        /// </summary>
        /// <param name="player">The Player GameObject containing the Rigidbody.</param>
        public virtual void Initialize(GameObject player)
        {
            _player = player;
            _rb = player.GetComponent<Rigidbody>();
        }
        #endregion

        #region Public API
        /// <summary>
        /// Checks <see cref="IsOnCooldown"/> before 
        /// triggering the custom logic in <see cref="OnExecute"/>.
        /// </summary>
        public void Execute()
        {
            if (IsOnCooldown) return;
            float staminaToTake = this is ToggleAbility ? _staminaLossRate : _staminaCost;
            if (_player.GetComponent<PlayerHandler>().Events.GetCurrentStamina?.Invoke() - staminaToTake < 0) return;
            OnExecute(); 
        }

        /// <summary>
        /// Ends the ability logic via <see cref="OnFinalise"/> and triggers the cooldown timer.
        /// </summary>
        public void Finalise()
        {
            OnFinalise();
            StartCooldown();
            Debug.Log("Finalise Executed");
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Implement custom ability logic 
        /// </summary>
        protected abstract void OnExecute();

        /// <summary>
        /// Implement  cleanup logic here
        /// </summary>
        protected virtual void OnFinalise() { }

        /// <summary>
        /// Manually sets the next available time the ability can be used.
        /// </summary>
        protected void StartCooldown() => _nextReadyTime = Time.time + cooldownTime;

        /// <summary>
        /// Allows for frame based code in abilities
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Tick(float deltaTime){ }
        
        /// <summary>
        /// Called to decrease the players stamina by the set rate (StaminaCost / StaminaPerSecond)
        /// </summary>
        protected virtual void UtiliseStamina()
        {
            float staminaToTake = this is ToggleAbility ? _staminaLossRate * Time.deltaTime : _staminaCost;
            _player.GetComponent<PlayerHandler>().Events.OnStaminaDecrease?.Invoke(staminaToTake);
        }
        #endregion

        #region Checks
        /// <summary>
        /// Returns <see langword="true"/> if the ability is currently on cooldown.
        /// </summary>
        public bool IsOnCooldown => Time.time < _nextReadyTime;

        /// <summary>
        /// Returns a value between 0 and 1 representing the remaining cooldown progress.
        /// </summary>
        public float CooldownNormalized => Mathf.Clamp01((_nextReadyTime - Time.time) / cooldownTime);
        #endregion
    }
}