using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniBonk.UI
{

    public class LevelUpPopUpManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _rewardSlot1;
        [SerializeField] private GameObject _rewardSlot2;
        [SerializeField] private GameObject _rewardSlot3;

        [SerializeField] private PopUpUnlockable[] _unlockables;
    
        private List<PopUpUnlockable> _usedUnlockables = new List<PopUpUnlockable>();
    
        #endregion
    
        void Start()
        {
            AssignRandomUnlockables();
        }

        void Update()
        {
        
        }
    
        private void AssignRandomUnlockables()
        {
            if (_unlockables == null || _unlockables.Length == 0)
            {
                Debug.LogWarning("No unlockables assigned to LevelUpPopUpManager!");
                return;
            }
        
            var rewardSlots = new[] { _rewardSlot1, _rewardSlot2, _rewardSlot3 };
        
            // Get available unlockables that haven't been used
            var availableForSelection = _unlockables.Except(_usedUnlockables).ToList();
        
            // If we've used all unlockables, reset the used list
            if (availableForSelection.Count < rewardSlots.Length)
            {
                _usedUnlockables.Clear();
                availableForSelection = _unlockables.ToList();
            }
        
            // Shuffle the available unlockables
            for (int i = 0; i < availableForSelection.Count; i++)
            {
                var randomIndex = Random.Range(i, availableForSelection.Count);
                (availableForSelection[i], availableForSelection[randomIndex]) = (availableForSelection[randomIndex], availableForSelection[i]);
            }
        
            // Assign unique unlockables to each reward slot
            for (int i = 0; i < rewardSlots.Length && i < availableForSelection.Count; i++)
            {
                var rewardSlot = rewardSlots[i];
                if (rewardSlot != null)
                {
                    var unlockController = rewardSlot.GetComponent<UnlockOptionController>();
                    if (unlockController != null && availableForSelection[i] != null)
                    {
                        var selectedUnlockable = availableForSelection[i];
                        unlockController.SetUnlockableData(selectedUnlockable);
                        _usedUnlockables.Add(selectedUnlockable);
                    }
                    else
                    {
                        Debug.LogWarning($"Reward slot {i + 1} is missing UnlockOptionController component or unlockable data is null!");
                    }
                }
            }
        }

        public void OnRewardClicked(GameObject reward)
        {
            var unlockController = reward.GetComponent<UnlockOptionController>();
            if (unlockController != null)
            {
                unlockController.ExecuteReward();
            }
        }
    
    }

}
