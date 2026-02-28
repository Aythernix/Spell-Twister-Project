using Minibonk.Management;
using Minibonk.MovementAbilities;
using UnityEngine;

namespace Minibonk.UI
{
    public class InventoryManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private InventorySlotController[] _inventorySlots = new InventorySlotController[3];
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            GameManager.Instance.EventHandler.OnAbilityEquipped += OnAbilityEquipped;
            GameManager.Instance.EventHandler.OnAbilitySlotTriggered += TriggerAbility;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion
        
        #region Private Methods

        private void OnAbilityEquipped(int inventoryIndex, MovementAbility movementAbility)
        {
            _inventorySlots[inventoryIndex].SetSlot(movementAbility);
        }

        private void TriggerAbility(int inventoryIndex)
        {
            _inventorySlots[inventoryIndex].OnAbilityTriggered();
        }
        #endregion
        
        #region Public API
        //
        #endregion
    }
}