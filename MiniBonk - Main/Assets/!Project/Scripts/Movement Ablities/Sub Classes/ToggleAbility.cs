namespace Minibonk.MovementAbilities
{
    public abstract class ToggleAbility : MovementAbility
    {
        protected bool _isActive { get; private set; }

        protected override void OnExecute()
        {
            _isActive = true;
            OnToggle(true);
        }

        protected override void OnFinalise()
        {
            if (!_isActive) return;
            _isActive = false;
            OnToggle(false);
        }

        /// <summary>
        /// Implement Custom Ability Logic
        /// </summary>
        /// <param name="state">The current state of the toggle, <see langword="true"/> or <see langword="false"/> </param> 
        protected abstract void OnToggle(bool state);
        
    }
}