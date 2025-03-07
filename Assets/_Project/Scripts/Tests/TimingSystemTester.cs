using UnityEngine;
using UnityEngine.UI;
using RoguelikeCombat.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace RoguelikeCombat.Tests
{
    public class TimingSystemTester : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TimingBarUI timingBar;
        [SerializeField] private ChainMeterUI chainMeter;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private TextMeshProUGUI chainText;

        [Header("Timing Settings")]
        [SerializeField] private float timingWindowDuration = 2f;
        [SerializeField] private float perfectWindowPercentage = 0.2f;
        [SerializeField] private float goodWindowPercentage = 0.4f;

        [Header("Chain Settings")]
        [SerializeField] private int maxChainLength = 5;
        [SerializeField] private float chainBreakDelay = 1.5f;

        private int currentChain;
        private bool isTimingActive;
        private float lastHitTime;

        private void Start()
        {
            chainMeter.Initialize(maxChainLength);
            StartNewTimingWindow();
        }

        private void Update()
        {
            if (isTimingActive && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                HandleTimingInput();
            }

            // Auto-break chain if too much time has passed
            if (currentChain > 0 && Time.time - lastHitTime > chainBreakDelay)
            {
                BreakChain();
            }
        }

        private void HandleTimingInput()
        {
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

            StartNewTimingWindow();
        }

        private void StartNewTimingWindow()
        {
            isTimingActive = true;
            timingBar.StartTimingWindow(timingWindowDuration);
        }

        private void BreakChain()
        {
            currentChain = 0;
            chainText.text = "Chain: 0";
            chainMeter.ResetChain();
        }
    }
}
