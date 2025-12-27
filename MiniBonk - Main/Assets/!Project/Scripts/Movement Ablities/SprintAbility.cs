using Minibonk.MovementAbilities;
using Minibonk.Player;
using UnityEngine;

namespace Minibonk.MovementAbilities
{
    [CreateAssetMenu(fileName = "Sprtint Ability", menuName = "Scriptable Objects/MovementAbilities")]
    public class SprintAbility : MovementAbility
    {
        #region variables

        [SerializeField] private float _sprintMultiplier = 1.5f;
        private PlayerMovement _movement;

        #endregion

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Initialize(GameObject player)
        {
            _movement = player.GetComponent<PlayerMovement>();
        }

        public override void Execute()
        {
            _movement.speedMultiplier = _sprintMultiplier;
        }
    }
}

