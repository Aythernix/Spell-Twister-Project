using Minibonk.Player;
using UnityEngine;


namespace Minibonk.MovementAbilities
{
   
    [CreateAssetMenu]
    public class Sprint : ToggleAbility
    {
        #region Variables

        [Header("Sprint Settings")]
        [SerializeField] private float _sprintMultipler;
    

        #endregion

        protected override void OnToggle(bool state)
        {
            switch (state)
            {
                case true:
                    _player.GetComponent<PlayerMovement>().speedMultiplier = _sprintMultipler;
                    _player.GetComponent<PlayerHandler>().PlayerEvents.OnSprintStarted?.Invoke();
                    break;
                case false:
                    _player.GetComponent<PlayerMovement>().speedMultiplier = 1;
                    _player.GetComponent<PlayerHandler>().PlayerEvents.OnSprintFinished?.Invoke();
                    break;
            }
        }
    }  
}

