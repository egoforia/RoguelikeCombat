using UnityEngine;
using UnityEngine.UI;
using RoguelikeCombat.Combat;

namespace RoguelikeCombat.UI
{
    public class TimingBarUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform barBackground;
        [SerializeField] private RectTransform markerRect;
        [SerializeField] private RectTransform perfectZoneRect;
        [SerializeField] private RectTransform goodZoneRect;
        
        [Header("Visual Settings")]
        [SerializeField] private Color perfectColor = Color.green;
        [SerializeField] private Color goodColor = Color.yellow;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color missColor = Color.red;
        [SerializeField] private float markerSpeed = 500f; // Pixels per second
        
        [Header("Animation")]
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private float pulseDuration = 0.2f;

        private Image markerImage;
        private bool isWindowActive;
        private float windowDuration;
        private float currentTime;
        private Vector2 startPosition;
        private Vector2 endPosition;

        private void Awake()
        {
            markerImage = markerRect.GetComponent<Image>();
            startPosition = new Vector2(-barBackground.rect.width / 2f, markerRect.anchoredPosition.y);
            endPosition = new Vector2(barBackground.rect.width / 2f, markerRect.anchoredPosition.y);
        }

        public void StartTimingWindow(float duration)
        {
            windowDuration = duration;
            currentTime = 0f;
            isWindowActive = true;
            markerRect.anchoredPosition = startPosition;
            SetMarkerColor(normalColor);
        }

        public void EndTimingWindow()
        {
            isWindowActive = false;
        }

        public void ShowTimingResult(TimingResult result)
        {
            Color resultColor = GetColorForResult(result);
            SetMarkerColor(resultColor);
            PulseMarker();
        }

        private void Update()
        {
            if (isWindowActive)
            {
                currentTime += Time.deltaTime;
                float progress = currentTime / windowDuration;
                UpdateMarkerPosition(progress);

                if (progress >= 1f)
                {
                    EndTimingWindow();
                }
            }
        }

        private void UpdateMarkerPosition(float progress)
        {
            Vector2 newPosition = Vector2.Lerp(startPosition, endPosition, progress);
            markerRect.anchoredPosition = newPosition;
        }

        private void SetMarkerColor(Color color)
        {
            if (markerImage != null)
            {
                markerImage.color = color;
            }
        }

        private Color GetColorForResult(TimingResult result)
        {
            switch (result)
            {
                case TimingResult.Perfect:
                    return perfectColor;
                case TimingResult.Good:
                    return goodColor;
                case TimingResult.Normal:
                    return normalColor;
                default:
                    return missColor;
            }
        }

        private void PulseMarker()
        {
            LeanTween.scale(markerRect.gameObject, Vector3.one * pulseScale, pulseDuration / 2f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() => {
                    LeanTween.scale(markerRect.gameObject, Vector3.one, pulseDuration / 2f)
                        .setEase(LeanTweenType.easeInQuad);
                });
        }
    }
}
