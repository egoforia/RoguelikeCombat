using UnityEngine;
using UnityEngine.Events;

namespace RoguelikeCombat.Combat
{
    public class TimingSystem : MonoBehaviour
    {
        [System.Serializable]
        public class TimingWindowEvent : UnityEvent<float> { }

        [Header("Timing Configuration")]
        [SerializeField] private float timingWindowDuration = 0.5f; // 500ms configurable window
        [SerializeField] private float perfectThreshold = 0.1f;
        [SerializeField] private float goodThreshold = 0.2f;

        public TimingWindowEvent onTimingWindowStart;
        public TimingWindowEvent onTimingWindowEnd;
        public TimingWindowEvent onTimingAttempt;

        private float windowStartTime;
        private bool isWindowActive;

        public void StartTimingWindow()
        {
            if (!isWindowActive)
            {
                windowStartTime = Time.time;
                isWindowActive = true;
                onTimingWindowStart?.Invoke(timingWindowDuration);
            }
        }

        public void AttemptTiming()
        {
            if (isWindowActive)
            {
                float timingAccuracy = CalculateTimingAccuracy();
                onTimingAttempt?.Invoke(timingAccuracy);
                EndTimingWindow();
            }
        }

        private float CalculateTimingAccuracy()
        {
            float elapsedTime = Time.time - windowStartTime;
            float centerPoint = timingWindowDuration / 2f;
            return Mathf.Abs(elapsedTime - centerPoint);
        }

        public TimingResult GetTimingResult(float accuracy)
        {
            if (accuracy <= perfectThreshold)
                return TimingResult.Perfect;
            if (accuracy <= goodThreshold)
                return TimingResult.Good;
            if (accuracy <= timingWindowDuration / 2f)
                return TimingResult.Normal;
            return TimingResult.Miss;
        }

        private void EndTimingWindow()
        {
            if (isWindowActive)
            {
                isWindowActive = false;
                onTimingWindowEnd?.Invoke(CalculateTimingAccuracy());
            }
        }

        private void Update()
        {
            if (isWindowActive)
            {
                float elapsedTime = Time.time - windowStartTime;
                if (elapsedTime >= timingWindowDuration)
                {
                    EndTimingWindow();
                }
            }
        }

        public bool IsWindowActive()
        {
            return isWindowActive;
        }

        public float GetWindowProgress()
        {
            if (!isWindowActive) return 0f;
            return (Time.time - windowStartTime) / timingWindowDuration;
        }
    }

    public enum TimingResult
    {
        Miss,
        Normal,
        Good,
        Perfect
    }
}
