namespace Minibonk.MovementAbilities
{
    public abstract class ToggleAbility : MovementAbility
    {
        protected bool _isActive;

        protected override void OnExecute()
        {
            _isActive = true;
            SetToggle(true);
        }

        protected override void OnFinalise()
        {
            if (!_isActive) return;
            _isActive = false;
            SetToggle(false);
        }

        /// <summary>
        /// Implement custom ability logic.
        /// </summary>
        protected abstract void SetToggle(bool state);
    }
}