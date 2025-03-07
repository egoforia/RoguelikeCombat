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
        
        [Header("Visual Settings")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color perfectColor = Color.green;
        [SerializeField] private Color goodColor = Color.yellow;
        [SerializeField] private Color missColor = Color.red;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private Ease pulseEase = Ease.OutBack;
        
        private Image fillBarImage;
        private float targetWidth;
        private Sequence currentAnimation;

        private void Awake()
        {
            fillBarImage = fillBar.GetComponent<Image>();
            DOTween.SetTweensCapacity(500, 50);
        }

        private void OnDestroy()
        {
            currentAnimation?.Kill();
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

        public void UpdateFillAmount(float amount)
        {
            targetWidth = amount * GetComponent<RectTransform>().rect.width;
            fillBar.DOSizeDelta(new Vector2(targetWidth, fillBar.sizeDelta.y), 0.1f)
                .SetEase(Ease.OutQuad);
        }

        public void ShowTimingResult(TimingResult result)
        {
            Color targetColor = GetColorForResult(result);
            
            // Kill any ongoing animation
            currentAnimation?.Kill();
            
            // Create a new animation sequence
            currentAnimation = DOTween.Sequence();
            
            // Scale up and change color
            currentAnimation.Append(fillBar.DOScale(Vector3.one * pulseScale, animationDuration / 2)
                .SetEase(pulseEase));
            currentAnimation.Join(fillBarImage.DOColor(targetColor, animationDuration / 2));
            
            // Scale down
            currentAnimation.Append(fillBar.DOScale(Vector3.one, animationDuration / 2)
                .SetEase(Ease.OutQuad));
            
            // Return to default color
            currentAnimation.Append(fillBarImage.DOColor(defaultColor, animationDuration)
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
