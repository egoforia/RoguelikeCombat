using UnityEngine;
using TMPro;
using RoguelikeCombat.Combat;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.UI
{
    public class CombatUIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TimingBarUI timingBarUI;
        [SerializeField] private ChainMeterUI chainMeterUI;
        [SerializeField] private TextMeshProUGUI chainCountText;
        [SerializeField] private TextMeshProUGUI damageMultiplierText;

        private CombatManager combatManager;
        private TimingSystem timingSystem;

        private void Awake()
        {
            combatManager = FindObjectOfType<CombatManager>();
            timingSystem = FindObjectOfType<TimingSystem>();
            
            if (timingSystem != null)
            {
                timingSystem.onTimingWindowStart.AddListener(OnTimingWindowStart);
                timingSystem.onTimingWindowEnd.AddListener(OnTimingWindowEnd);
                timingSystem.onTimingAttempt.AddListener(OnTimingAttempt);
            }
        }

        private void OnTimingWindowStart(float duration)
        {
            timingBarUI?.StartTimingWindow(duration);
        }

        private void OnTimingWindowEnd(float accuracy)
        {
            timingBarUI?.EndTimingWindow();
            UpdateChainUI();
        }

        private void OnTimingAttempt(float accuracy)
        {
            TimingResult result = timingSystem.GetTimingResult(accuracy);
            timingBarUI?.ShowTimingResult(result);
            chainMeterUI?.UpdateChainVisual(combatManager.GetCurrentChainCount(), result);
        }

        private void UpdateChainUI()
        {
            int chainCount = combatManager.GetCurrentChainCount();
            float multiplier = combatManager.GetCurrentDamageMultiplier();

            if (chainCountText != null)
                chainCountText.text = $"Chain: {chainCount}";
            
            if (damageMultiplierText != null)
                damageMultiplierText.text = $"x{multiplier:F1}";
        }

        private void OnDestroy()
        {
            if (timingSystem != null)
            {
                timingSystem.onTimingWindowStart.RemoveListener(OnTimingWindowStart);
                timingSystem.onTimingWindowEnd.RemoveListener(OnTimingWindowEnd);
                timingSystem.onTimingAttempt.RemoveListener(OnTimingAttempt);
            }
        }
    }
}
