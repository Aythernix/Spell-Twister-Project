using Minibonk.Player;
using UnityEngine;


namespace Minibonk.MovementAbilities
{
   
    [CreateAssetMenu]
    public class Sprint : ToggleAbility
    {
        #region Variables

        [SerializeField] private float _sprintMultipler;
    

        #endregion

        protected override void SetToggle(bool state)
        {
            switch (state)
            {
                case true:
                    _player.GetComponent<PlayerMovement>().speedMultiplier = _sprintMultipler;
                    break;
                case false:
                    _player.GetComponent<PlayerMovement>().speedMultiplier = 1;
                    break;
            }
        }
    }  
}

