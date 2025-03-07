using UnityEngine;
using UnityEngine.InputSystem;
using RoguelikeCombat.Combat;
using RoguelikeCombat.UI;

namespace RoguelikeCombat.Tests
{
    public class TimingSystemTester : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TimingBarUI timingBarUI;
        [SerializeField] private ChainMeterUI chainMeterUI;

        [Header("Test Settings")]
        [SerializeField] private float timingWindowDuration = 1f;
        [SerializeField] private KeyCode testKey = KeyCode.Space;
        [SerializeField] private float perfectThreshold = 0.1f;
        [SerializeField] private float goodThreshold = 0.2f;

        private bool isWindowActive;
        private float windowStartTime;
        private int currentChain;

        private void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                if (!isWindowActive)
                {
                    StartTimingWindow();
                }
                else
                {
                    CheckTiming();
                }
            }
        }

        private void StartTimingWindow()
        {
            isWindowActive = true;
            windowStartTime = Time.time;
            timingBarUI.StartTimingWindow(timingWindowDuration);

            // Set timing zones
            float perfectStart = 0.4f;
            float perfectEnd = 0.6f;
            float goodStart = 0.3f;
            float goodEnd = 0.7f;
            timingBarUI.SetZones(goodStart, goodEnd, perfectStart, perfectEnd);
        }

        private void CheckTiming()
        {
            float elapsedTime = Time.time - windowStartTime;
            float normalizedTime = elapsedTime / timingWindowDuration;
            TimingResult result;

            // Calculate timing result
            if (normalizedTime >= 0.4f && normalizedTime <= 0.6f)
            {
                result = TimingResult.Perfect;
                currentChain++;
            }
            else if (normalizedTime >= 0.3f && normalizedTime <= 0.7f)
            {
                result = TimingResult.Good;
                currentChain++;
            }
            else
            {
                result = TimingResult.Miss;
                currentChain = 0;
            }

            // Show results
            timingBarUI.ShowTimingResult(result);
            chainMeterUI.UpdateChainVisual(currentChain, result);

            // End window
            isWindowActive = false;
            timingBarUI.EndTimingWindow();
        }
    }
}
