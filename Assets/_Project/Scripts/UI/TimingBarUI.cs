using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RoguelikeCombat.Combat;

namespace RoguelikeCombat.UI 
{
    public class TimingBarUI : MonoBehaviour 
    {
        [Header("UI References")]
        [SerializeField] private RectTransform fillBar;
        [SerializeField] private RectTransform perfectZone;
        [SerializeField] private RectTransform goodZone;
        [SerializeField] private RectTransform marker;
        
        [Header("Visual Settings")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color perfectColor = Color.green;
        [SerializeField] private Color goodColor = Color.yellow;
        [SerializeField] private Color missColor = Color.red;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private Ease pulseEase = Ease.OutBack;
        
        private Image markerImage;
        private Image fillBarImage;
        private float startX;
        private float endX;
        private Sequence currentAnimation;
        private Tweener markerMovement;
        private float markerStartTime;
        private float windowDuration;
        private bool isWindowActive;

        private void Awake()
        {
            fillBarImage = fillBar.GetComponent<Image>();
            markerImage = marker.GetComponent<Image>();
            DOTween.SetTweensCapacity(500, 50);
            
            RectTransform rect = GetComponent<RectTransform>();
            startX = -rect.rect.width / 2f;
            endX = rect.rect.width / 2f;
            
            // Center the timing zones
            float perfectWidth = perfectZone.rect.width;
            float goodWidth = goodZone.rect.width;
            perfectZone.anchoredPosition = new Vector2(0, 0);
            goodZone.anchoredPosition = new Vector2(0, 0);
        }

        private void OnDestroy()
        {
            currentAnimation?.Kill();
            markerMovement?.Kill();
        }

        public void StartTimingWindow(float duration)
        {
            windowDuration = duration;
            markerStartTime = Time.time;
            isWindowActive = true;
            
            // Reset marker position
            marker.anchoredPosition = new Vector2(startX, marker.anchoredPosition.y);
            markerImage.color = defaultColor;
            
            // Kill any existing movement
            markerMovement?.Kill();
            
            // Start new movement
            markerMovement = marker.DOAnchorPosX(endX, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => EndTimingWindow());

            // Reset fill bar
            fillBar.sizeDelta = new Vector2(0, fillBar.sizeDelta.y);
            fillBar.DOSizeDelta(new Vector2(fillBar.parent.GetComponent<RectTransform>().rect.width, fillBar.sizeDelta.y), duration)
                .SetEase(Ease.Linear);
        }

        public float GetTimingResult()
        {
            if (!isWindowActive) return 1f;

            // Calculate normalized position (0 = perfect center, 1 = edges)
            float normalizedPosition = Mathf.Abs(marker.anchoredPosition.x / (endX - startX));
            
            // End the window
            EndTimingWindow();
            
            return normalizedPosition;
        }

        private void EndTimingWindow()
        {
            isWindowActive = false;
            markerMovement?.Kill();
        }

        public void SetZones(float perfectStart, float perfectEnd, float goodStart, float goodEnd)
        {
            float totalWidth = GetComponent<RectTransform>().rect.width;
            
            // Set perfect zone position and width
            float perfectWidth = (perfectEnd - perfectStart) * totalWidth;
            perfectZone.sizeDelta = new Vector2(perfectWidth, perfectZone.sizeDelta.y);
            perfectZone.anchoredPosition = new Vector2(perfectStart * totalWidth, perfectZone.anchoredPosition.y);
            
            // Set good zone position and width
            float goodWidth = (goodEnd - goodStart) * totalWidth;
            goodZone.sizeDelta = new Vector2(goodWidth, goodZone.sizeDelta.y);
            goodZone.anchoredPosition = new Vector2(goodStart * totalWidth, goodZone.anchoredPosition.y);
        }

        public void ShowTimingResult(TimingResult result)
        {
            Color targetColor = GetColorForResult(result);
            
            // Kill any ongoing animation
            currentAnimation?.Kill();
            
            // Create a new animation sequence
            currentAnimation = DOTween.Sequence();
            
            // Scale up and change color
            currentAnimation.Append(marker.DOScale(Vector3.one * pulseScale, animationDuration / 2)
                .SetEase(pulseEase));
            currentAnimation.Join(markerImage.DOColor(targetColor, animationDuration / 2));
            
            // Scale down
            currentAnimation.Append(marker.DOScale(Vector3.one, animationDuration / 2)
                .SetEase(Ease.OutQuad));
            
            // Return to default color
            currentAnimation.Append(markerImage.DOColor(defaultColor, animationDuration)
                .SetEase(Ease.InOutQuad));
        }

        private Color GetColorForResult(TimingResult result)
        {
            switch (result)
            {
                case TimingResult.Perfect:
                    return perfectColor;
                case TimingResult.Good:
                    return goodColor;
                default:
                    return missColor;
            }
        }
    }
}
