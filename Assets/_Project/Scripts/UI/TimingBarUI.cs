using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
        private Coroutine pulseCoroutine;

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
            PulseMarker(result);
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

        private void PulseMarker(TimingResult result)
        {
            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
            }
            pulseCoroutine = StartCoroutine(PulseMarkerCoroutine(result));
        }

        private IEnumerator PulseMarkerCoroutine(TimingResult result)
        {
            Color targetColor = GetColorForResult(result);
            Vector3 originalScale = markerRect.localScale;
            Vector3 pulseScaleVector = originalScale * pulseScale;
            float elapsedTime = 0f;

            // Scale up
            while (elapsedTime < pulseDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (pulseDuration / 2);
                markerRect.localScale = Vector3.Lerp(originalScale, pulseScaleVector, t);
                markerImage.color = Color.Lerp(markerImage.color, targetColor, t);
                yield return null;
            }

            elapsedTime = 0f;

            // Scale down
            while (elapsedTime < pulseDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / (pulseDuration / 2);
                markerRect.localScale = Vector3.Lerp(pulseScaleVector, originalScale, t);
                yield return null;
            }

            markerRect.localScale = originalScale;
            yield return new WaitForSeconds(0.1f);
            
            // Fade back to default color
            elapsedTime = 0f;
            while (elapsedTime < pulseDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / pulseDuration;
                markerImage.color = Color.Lerp(targetColor, normalColor, t);
                yield return null;
            }
            
            markerImage.color = normalColor;
        }
    }
}
