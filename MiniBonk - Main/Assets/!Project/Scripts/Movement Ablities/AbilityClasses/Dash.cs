using UnityEngine;

namespace Minibonk.MovementAbilities
{
    [CreateAssetMenu]
    public class Dash : ImpulseAbility
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        protected override void ApplyImpulse()
        {
            _rb.AddForce(_player.transform.forward * 100, ForceMode.Impulse);
        }
    }
}

