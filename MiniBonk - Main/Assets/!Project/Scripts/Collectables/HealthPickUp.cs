using Minibonk.Management;
using UnityEngine;

namespace Minibonk.Collectables
{
    public class HealthPickUp : Collectable.Collectable
    {
        [SerializeField] private float _healthAmount;

        protected override void Collected()
        {
            GameManager.Instance.currentLevelManager.playerHandler.PlayerEvents.OnPlayerHeal?.Invoke(_healthAmount * GameManager.Instance.ChaosMultiplier());
        }
    }
}
