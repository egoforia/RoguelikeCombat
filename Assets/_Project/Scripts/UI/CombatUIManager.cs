using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.UI
{
    public class CombatUIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TimingBarUI timingBar;
        [SerializeField] private ChainMeterUI chainMeter;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private TextMeshProUGUI chainText;

        [Header("Chain Settings")]
        [SerializeField] private int maxChainLength = 5;
        [SerializeField] private float chainBreakDelay = 1.5f;

        [Header("Timing Settings")]
        [SerializeField] private float timingWindowDuration = 2f;
        [SerializeField] private float perfectWindowPercentage = 0.2f;
        [SerializeField] private float goodWindowPercentage = 0.4f;

        private int currentChain;
        private float lastHitTime;
        private bool isTimingActive;

        private void Start()
        {
            chainMeter.Initialize(maxChainLength);
            feedbackText.text = "";
            chainText.text = "Chain: 0";
        }

        private void Update()
        {
            // Auto-break chain if too much time has passed
            if (currentChain > 0 && Time.time - lastHitTime > chainBreakDelay)
            {
                BreakChain();
            }
        }

        public void StartTimingWindow()
        {
            isTimingActive = true;
            timingBar.StartTimingWindow(timingWindowDuration);
        }

        public void HandleTimingInput()
        {
            if (!isTimingActive) return;

            float result = timingBar.GetTimingResult();
            string feedback;
            Color feedbackColor;

            if (result <= perfectWindowPercentage)
            {
                feedback = "Perfect!";
                feedbackColor = Color.green;
                currentChain++;
                chainMeter.AddSegment();
            }
            else if (result <= goodWindowPercentage)
            {
                feedback = "Good";
                feedbackColor = Color.yellow;
                currentChain++;
                chainMeter.AddSegment();
            }
            else
            {
                feedback = "Miss";
                feedbackColor = Color.red;
                BreakChain();
            }

            feedbackText.text = feedback;
            feedbackText.color = feedbackColor;
            chainText.text = $"Chain: {currentChain}";
            lastHitTime = Time.time;
            isTimingActive = false;
        }

        private void BreakChain()
        {
            currentChain = 0;
            chainText.text = "Chain: 0";
            chainMeter.ResetChain();
        }
    }
}
