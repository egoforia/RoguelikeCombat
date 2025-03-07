using UnityEngine;

namespace RoguelikeCombat.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [Header("Timing Configuration")]
        [SerializeField] private float baseTimingWindow = 0.5f; // 500ms default
        [SerializeField] private float perfectTimingThreshold = 0.1f; // 100ms for perfect timing
        [SerializeField] private float goodTimingThreshold = 0.2f; // 200ms for good timing

        [Header("Chain Configuration")]
        [SerializeField] private float baseMultiplier = 1.0f;
        [SerializeField] private float goodTimingMultiplier = 1.2f;
        [SerializeField] private float perfectTimingMultiplier = 1.5f;
        [SerializeField] private float chainBonusMultiplier = 0.2f;
        [SerializeField] private float maxChainMultiplier = 2.5f;

        private int currentChainCount = 0;
        private float currentDamageMultiplier;

        private void Start()
        {
            ResetChain();
        }

        public void ProcessAttack(float timingAccuracy)
        {
            if (IsTimingValid(timingAccuracy))
            {
                HandleSuccessfulHit(timingAccuracy);
            }
            else
            {
                ResetChain();
            }
        }

        private bool IsTimingValid(float accuracy)
        {
            return Mathf.Abs(accuracy) <= baseTimingWindow;
        }

        private void HandleSuccessfulHit(float accuracy)
        {
            currentChainCount++;
            
            if (IsPerfectTiming(accuracy))
            {
                ApplyPerfectHit();
            }
            else if (IsGoodTiming(accuracy))
            {
                ApplyGoodHit();
            }
            else
            {
                ApplyNormalHit();
            }
        }

        private bool IsPerfectTiming(float accuracy)
        {
            return Mathf.Abs(accuracy) <= perfectTimingThreshold;
        }

        private bool IsGoodTiming(float accuracy)
        {
            return Mathf.Abs(accuracy) <= goodTimingThreshold;
        }

        private void ApplyPerfectHit()
        {
            float chainBonus = Mathf.Min(chainBonusMultiplier * (currentChainCount - 1), maxChainMultiplier - perfectTimingMultiplier);
            currentDamageMultiplier = perfectTimingMultiplier + chainBonus;
        }

        private void ApplyGoodHit()
        {
            currentDamageMultiplier = goodTimingMultiplier;
        }

        private void ApplyNormalHit()
        {
            currentDamageMultiplier = baseMultiplier;
        }

        private void ResetChain()
        {
            currentChainCount = 0;
            currentDamageMultiplier = baseMultiplier;
        }

        public float GetCurrentDamageMultiplier()
        {
            return currentDamageMultiplier;
        }

        public int GetCurrentChainCount()
        {
            return currentChainCount;
        }
    }
}
