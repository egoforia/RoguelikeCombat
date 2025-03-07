using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RoguelikeCombat.Combat;

namespace RoguelikeCombat.UI
{
    public class ChainMeterUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform segmentsContainer;
        [SerializeField] private GameObject segmentPrefab;
        [SerializeField] private int maxSegments = 4;

        [Header("Visual Settings")]
        [SerializeField] private Color perfectColor = Color.green;
        [SerializeField] private Color goodColor = Color.yellow;
        [SerializeField] private Color failedColor = Color.red;
        [SerializeField] private Color inactiveColor = Color.gray;
        [SerializeField] private float segmentScale = 1f;
        [SerializeField] private float rotationOffset = 90f;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float pulseScale = 1.2f;
        [SerializeField] private Ease pulseEase = Ease.OutBack;
        
        private Image[] segments;
        private int currentChain;
        private Sequence[] segmentAnimations;

        private void Awake()
        {
            InitializeSegments();
            DOTween.SetTweensCapacity(500, 50);
        }

        private void OnDestroy()
        {
            if (segmentAnimations != null)
            {
                foreach (var anim in segmentAnimations)
                {
                    anim?.Kill();
                }
            }
        }

        private void InitializeSegments()
        {
            segments = new Image[maxSegments];
            segmentAnimations = new Sequence[maxSegments];
            float angleStep = 360f / maxSegments;

            for (int i = 0; i < maxSegments; i++)
            {
                GameObject segment = Instantiate(segmentPrefab, segmentsContainer);
                segment.transform.localPosition = Vector3.zero;
                
                // Position segments in a circle
                float angle = i * angleStep + rotationOffset;
                segment.transform.localRotation = Quaternion.Euler(0, 0, angle);
                segment.transform.localScale = Vector3.one * segmentScale;
                
                segments[i] = segment.GetComponent<Image>();
                segments[i].color = inactiveColor;
            }
        }

        public void UpdateChainVisual(int chainCount, TimingResult result)
        {
            currentChain = Mathf.Min(chainCount, maxSegments);

            // Update active segments
            for (int i = 0; i < maxSegments; i++)
            {
                if (i < currentChain)
                {
                    Color targetColor = GetColorForResult(result);
                    segments[i].DOColor(targetColor, animationDuration / 2);
                    PulseSegment(i);
                }
                else
                {
                    segments[i].DOColor(inactiveColor, animationDuration);
                }
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
                default:
                    return failedColor;
            }
        }

        private void PulseSegment(int index)
        {
            // Kill any ongoing animation for this segment
            segmentAnimations[index]?.Kill();
            
            Transform segmentTransform = segments[index].transform;
            Vector3 originalScale = Vector3.one * segmentScale;
            Vector3 pulseScaleVector = originalScale * pulseScale;

            // Create new animation sequence
            segmentAnimations[index] = DOTween.Sequence();
            
            // Scale up
            segmentAnimations[index].Append(segmentTransform.DOScale(pulseScaleVector, animationDuration / 2)
                .SetEase(pulseEase));
            
            // Scale down
            segmentAnimations[index].Append(segmentTransform.DOScale(originalScale, animationDuration / 2)
                .SetEase(Ease.OutQuad));
        }

        public void ResetChain()
        {
            currentChain = 0;
            foreach (var segment in segments)
            {
                segment.DOColor(inactiveColor, animationDuration);
            }
        }
    }
}
