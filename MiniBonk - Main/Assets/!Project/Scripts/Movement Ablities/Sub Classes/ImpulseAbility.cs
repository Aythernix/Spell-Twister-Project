namespace Minibonk.MovementAbilities
{
    public abstract class ImpulseAbility : MovementAbility
    {
        protected override void OnExecute()
        {
            ApplyImpulse();
            Finalise(); 
        }

        /// <summary>
        /// Implement one time logic.
        /// </summary>
        protected abstract void ApplyImpulse();
    }
}